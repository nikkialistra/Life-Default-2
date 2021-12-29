﻿using System;
using Game.Units.Unit;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Units.Services
{
    public class UnitsGenerator : IInitializable, IDisposable
    {
        private readonly UnitFacade.Factory _factory;

        private readonly Camera _camera;
        
        private readonly PlayerInput _playerInput;

        private InputAction _generateUnitAction;
        private InputAction _positionAction;

        public UnitsGenerator(UnitFacade.Factory factory, Camera camera, PlayerInput playerInput)
        {
            _factory = factory;
            _camera = camera;
            _playerInput = playerInput;
        }

        private void GenerateUnit(InputAction.CallbackContext context)
        {
            var mousePosition = _positionAction.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, _camera.nearClipPlane));
            if (Physics.Raycast(ray, out var hit))
            {
                _factory.Create(hit.point);
            }
        }

        public void Initialize()
        {
            _generateUnitAction = _playerInput.actions.FindAction("TestingGenerateUnit");
            _positionAction = _playerInput.actions.FindAction("Position");
            
            _generateUnitAction.started += GenerateUnit;
        }

        public void Dispose()
        {
            _generateUnitAction.started -= GenerateUnit;
        }
    }
}