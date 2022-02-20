﻿using System;
using UI.Menus.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Menus.Primary
{
    public class GameMenuView : IMenuView
    {
        private readonly VisualElement _root;
        private readonly IHideNotify _hideNotify;

        private readonly TemplateContainer _tree;

        private readonly Button _resume;
        private readonly Button _save;
        private readonly Button _saveAs;
        private readonly Button _load;
        private readonly Button _settings;
        private readonly Button _mainMenu;
        private readonly Button _exitGame;

        private SettingsView _settingsView;

        public bool Shown { get; private set; }

        public GameMenuView(VisualElement root, IHideNotify hideNotify)
        {
            _root = root;
            _hideNotify = hideNotify;

            var template = Resources.Load<VisualTreeAsset>("UI/Markup/Menus/GameMenu");
            _tree = template.CloneTree();

            _resume = _tree.Q<Button>("resume");
            _save = _tree.Q<Button>("save");
            _saveAs = _tree.Q<Button>("save-as");
            _load = _tree.Q<Button>("load");
            _settings = _tree.Q<Button>("settings");
            _mainMenu = _tree.Q<Button>("main-menu");
            _exitGame = _tree.Q<Button>("exit-game");
        }

        public event Action Pausing;
        public event Action Resuming;

        public void ShowSelf()
        {
            _hideNotify.HideCurrentMenu += HideSelf;

            Shown = true;
            _root.Add(_tree);
            Time.timeScale = 0;

            _resume.clicked += HideSelf;
            _settings.clicked += Settings;
            _mainMenu.clicked += MainMenu;
            _exitGame.clicked += ExitGame;

            Pausing?.Invoke();
        }

        public void HideSelf()
        {
            _hideNotify.HideCurrentMenu -= HideSelf;

            Time.timeScale = 1;

            Shown = false;
            _root.Remove(_tree);

            _resume.clicked -= HideSelf;
            _settings.clicked -= Settings;
            _mainMenu.clicked -= MainMenu;
            _exitGame.clicked -= ExitGame;

            Resuming?.Invoke();
        }

        private void Settings()
        {
            HideSelf();

            _settingsView ??= new SettingsView(_root, this, _hideNotify);
            _settingsView.ShowSelf();
        }

        private static void MainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        private static void ExitGame()
        {
            Application.Quit();
        }
    }
}
