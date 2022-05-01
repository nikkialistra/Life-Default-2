﻿using System;
using System.Collections;
using General;
using General.Interfaces;
using General.Selecting;
using Sirenix.OdinInspector;
using Units.Ancillaries;
using UnityEngine;

namespace Units
{
    public class UnitSelection : MonoBehaviour, ISelectable
    {
        [Required]
        [SerializeField] private HoverIndicator _hoverIndicator;
        [Required]
        [SerializeField] private HealthBars _healthBars;

        private float _timeToHideHover;

        private bool _hovered;
        private bool _selected;
        private bool _activated;
        
        private Coroutine _hoveringCoroutine;
        
        private SelectingInput _selectingInput;

        public event Action Selected;
        public event Action Deselected;

        private void Start()
        {
            _timeToHideHover = GlobalParameters.Instance.TimeToHideHover;
        }

        public void Flash()
        {
            
        }

        public void Select()
        {
            _selected = true;
            HideHovering();
            
            Selected?.Invoke();
        }

        public void Deselect()
        {
            _selected = false;
            HideHovering();
            
            Deselected?.Invoke();
        }

        public void StopDisplay()
        {
            _activated = false;
            HideHovering();
        }

        public void Hover()
        {
            if (_hovered || _selected || !_activated)
            {
                return;
            }

            _hovered = true;

            if (_hoveringCoroutine == null)
            {
                _hoveringCoroutine = StartCoroutine(Hovering());
            }
        }
        
        public void Activate()
        {
            _activated = true;
        }

        public void Deactivate()
        {
            _activated = false;
        }

        private IEnumerator Hovering()
        {
            ShowHovering();
            
            while (true)
            {
                _hovered = false;

                yield return new WaitForSecondsRealtime(_timeToHideHover);

                if (!_hovered)
                {
                    HideHovering();
                    break;
                }
            }

            _hoveringCoroutine = null;
        }

        private void ShowHovering()
        {
            _hoverIndicator.Activate();
            _healthBars.Hovered = true;
        }

        private void HideHovering()
        {
            _hoverIndicator.Deactivate();
            _healthBars.Hovered = false;
        }
    }
}
