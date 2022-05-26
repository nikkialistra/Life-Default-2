﻿using Colonists;
using UI.Game.GameLook.Components.Info.ColonistTabs;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.GameLook.Components.Info.ColonistInfo
{
    [RequireComponent(typeof(ColonistInfoView))]
    [RequireComponent(typeof(ColonistInfoTab))]
    public class ColonistDetailTabs : MonoBehaviour
    {
        private Toggle _equipment;
        private Toggle _stats;
        private Toggle _skills;
        private Toggle _info;

        private ColonistInfoView _colonistInfoView;
        
        private ColonistInfoTab _colonistInfoTab;

        private void Awake()
        {
            _colonistInfoView = GetComponent<ColonistInfoView>();
            
            _colonistInfoTab = GetComponent<ColonistInfoTab>();
        }

        public void Initialize(VisualElement tree)
        {
            _equipment = tree.Q<Toggle>("equipment");
            _stats = tree.Q<Toggle>("stats");
            _skills = tree.Q<Toggle>("skills");
            _info = tree.Q<Toggle>("info");
        }

        public void BindSelf()
        {
            _equipment.RegisterValueChangedCallback(OnEquipmentToggle);
            _stats.RegisterValueChangedCallback(OnStatsToggle);
            _skills.RegisterValueChangedCallback(OnSkillsToggle);
            _info.RegisterValueChangedCallback(OnInfoToggle);
        }

        public void UnbindSelf()
        {
            _equipment.UnregisterValueChangedCallback(OnEquipmentToggle);
            _stats.UnregisterValueChangedCallback(OnStatsToggle);
            _skills.UnregisterValueChangedCallback(OnSkillsToggle);
            _info.UnregisterValueChangedCallback(OnInfoToggle);
        }
        
        public void FillIn(Colonist colonist)
        {
            if (_colonistInfoTab.Shown)
            {
                _colonistInfoTab.FillIn(colonist);
            }
        }

        private void OnEquipmentToggle(ChangeEvent<bool> changeEvent)
        {
            UncheckToggles();
            _equipment.SetValueWithoutNotify(changeEvent.newValue);
        }

        private void OnStatsToggle(ChangeEvent<bool> changeEvent)
        {
            UncheckToggles();
            _stats.SetValueWithoutNotify(changeEvent.newValue);
        }

        private void OnSkillsToggle(ChangeEvent<bool> changeEvent)
        {
            UncheckToggles();
            _skills.SetValueWithoutNotify(changeEvent.newValue);
        }

        private void OnInfoToggle(ChangeEvent<bool> changeEvent)
        {
            UncheckToggles();
            _info.SetValueWithoutNotify(changeEvent.newValue);

            ToggleInfo();
        }

        private void ToggleInfo()
        {
            if (_colonistInfoTab.Shown)
            {
                _colonistInfoTab.HideSelf();
            }
            else
            {
                HideAll();
                _colonistInfoTab.FillIn(_colonistInfoView.Colonist);
                _colonistInfoTab.ShowSelf();
            }
        }

        private void UncheckToggles()
        {
            _equipment.SetValueWithoutNotify(false);
            _stats.SetValueWithoutNotify(false);
            _skills.SetValueWithoutNotify(false);
            _info.SetValueWithoutNotify(false);
        }

        private void HideAll()
        {
            _colonistInfoTab.HideSelf();
        }
    }
}