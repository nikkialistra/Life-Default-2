﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kernel.UI.MenuViews
{
    public class GameMenuView : IMenuView
    {
        public event Action Resuming;
        public bool Shown { get; private set; }
        
        private VisualElement _root;

        private TemplateContainer _tree;

        private Button _resume;
        private Button _exitGame;

        public GameMenuView(VisualElement root)
        {
            _root = root;
            
            var template = Resources.Load<VisualTreeAsset>("UI/Markup/GameMenu");
            _tree = template.CloneTree();

            _resume = _tree.Q<Button>("resume");
            _exitGame = _tree.Q<Button>("exit_game");
        }

        public void ShowSelf()
        {
            _root.Add(_tree);
            Shown = true;
            Time.timeScale = 0;

            _resume.clicked += Resume;
            _exitGame.clicked += ExitGame;
        }

        public void HideSelf()
        {
            _root.Remove(_tree);
            Shown = false;
            Time.timeScale = 1;

            _resume.clicked -= Resume;
            _exitGame.clicked -= ExitGame;
        }

        private void Resume()
        {
            Resuming?.Invoke();
            Time.timeScale = 1;
            HideSelf();
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}