﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Zenject;

namespace UI.Menus
{
    [RequireComponent(typeof(UIDocument))]
    public class MenuViews : MonoBehaviour
    {
        private VisualElement _root;

        private GameMenuView _gameMenuView;

        private PlayerInput _playerInput;

        private InputAction _showMenuAction;
        private InputAction _hideMenuAction;

        [Inject]
        public void Construct(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public event Action HideCurrentMenu;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _gameMenuView = new GameMenuView(_root, this);

            _showMenuAction = _playerInput.actions.FindAction("Show Menu");
            _hideMenuAction = _playerInput.actions.FindAction("Hide Menu");
        }

        private void OnEnable()
        {
            _gameMenuView.Resuming += Resuming;
            _showMenuAction.started += ShowMenu;
            _hideMenuAction.started += HideMenu;
        }

        private void OnDisable()
        {
            _gameMenuView.Resuming -= Resuming;
            _showMenuAction.started -= ShowMenu;
            _hideMenuAction.started -= HideMenu;
        }

        private void Resuming()
        {
            _playerInput.SwitchCurrentActionMap("Management");
        }

        private void ShowMenu(InputAction.CallbackContext context)
        {
            _gameMenuView.ShowSelf();
            _playerInput.SwitchCurrentActionMap("Menus");
        }

        private void HideMenu(InputAction.CallbackContext context)
        {
            if (_gameMenuView.Shown)
            {
                _playerInput.SwitchCurrentActionMap("Management");
            }

            HideCurrentMenu?.Invoke();
        }
    }
}
