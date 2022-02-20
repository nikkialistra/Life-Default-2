﻿using System;
using UI.Menus.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

namespace UI.Menus.Primary
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuView : MonoBehaviour, IMenuView, IHideNotify
    {
        private VisualElement _tree;

        private Button _newGame;
        private Button _loadGame;
        private Button _settings;
        private Button _exitGame;

        private VisualElement _background;
        private VisualElement _buttons;

        private SettingsView _settingsView;

        private PlayerInput _playerInput;

        private InputAction _hideMenuAction;

        [Inject]
        public void Construct(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public event Action HideCurrentMenu;

        private void Awake()
        {
            _tree = GetComponent<UIDocument>().rootVisualElement;
            _background = _tree.Q<VisualElement>("background");
            _buttons = _tree.Q<VisualElement>("buttons");

            _newGame = _tree.Q<Button>("new-game");
            _loadGame = _tree.Q<Button>("load-game");
            _settings = _tree.Q<Button>("settings");
            _exitGame = _tree.Q<Button>("exit-game");

            _hideMenuAction = _playerInput.actions.FindAction("Hide Menu");
        }

        private void Start()
        {
            _playerInput.SwitchCurrentActionMap("Menus");
        }

        private void OnEnable()
        {
            _newGame.clicked += NewGame;
            _loadGame.clicked += LoadGame;
            _settings.clicked += Settings;
            _exitGame.clicked += ExitGame;

            _hideMenuAction.started += HideMenu;
        }

        private void OnDisable()
        {
            _newGame.clicked -= NewGame;
            _loadGame.clicked -= LoadGame;
            _settings.clicked -= Settings;
            _exitGame.clicked -= ExitGame;

            _hideMenuAction.started -= HideMenu;
        }

        public void ShowSelf()
        {
            _background.Add(_buttons);
        }

        public void HideSelf()
        {
            _background.Remove(_buttons);
        }

        private void HideMenu(InputAction.CallbackContext context)
        {
            HideCurrentMenu?.Invoke();
        }

        private static void NewGame()
        {
            SceneManager.LoadScene("Forest", LoadSceneMode.Single);
        }

        private static void LoadGame() { }

        private void Settings()
        {
            HideSelf();

            _settingsView ??= new SettingsView(_background, this, this);
            _settingsView.ShowSelf();
        }

        private static void ExitGame()
        {
            Application.Quit();
        }
    }
}