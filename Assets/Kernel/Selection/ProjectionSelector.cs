﻿using System;
using System.Collections.Generic;
using Game.Units.Scripts;
using Game.Units.Scripts.Services;
using Kernel.Types;
using UnityEngine;

namespace Kernel.Selection
{
    public class ProjectionSelector
    {
        private readonly UnitRepository _unitRepository;
        private readonly Camera _camera;

        private IEnumerable<Unit> _gameObjects;

        public ProjectionSelector(UnitRepository unitRepository, Camera camera)
        {
            _unitRepository = unitRepository;
            _camera = camera;
        }

        public IEnumerable<ISelectable> SelectInScreenSpace(Rect rect)
        {
            _gameObjects = _unitRepository.GetObjects();
            
            if (_gameObjects == null)
                throw new NullReferenceException();
            
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.GetComponent<ISelectable>() == null) continue;
                
                var screenPoint = _camera.WorldToScreenPoint(gameObject.transform.position);

                if (rect.Contains(screenPoint))
                {
                    yield return gameObject.GetComponent<ISelectable>();
                }
            }
        }
    }
}