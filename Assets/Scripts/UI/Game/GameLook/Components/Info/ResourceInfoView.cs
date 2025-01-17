﻿using System.Collections.Generic;
using ResourceManagement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.GameLook.Components.Info
{
    [RequireComponent(typeof(InfoPanelView))]
    public class ResourceInfoView : MonoBehaviour
    {
        [Required]
        [SerializeField] private VisualTreeAsset _asset;

        private InfoPanelView _parent;
        private TemplateContainer _tree;

        private Label _name;

        private readonly List<VisualElement> _rows = new(2);
        private readonly List<Label> _rowNames = new(2);
        private readonly List<Label> _rowValues = new(2);

        private Resource _resource;

        private bool _shown;

        private void Awake()
        {
            _parent = GetComponent<InfoPanelView>();

            _tree = _asset.CloneTree();
            _tree.pickingMode = PickingMode.Ignore;

            _name = _tree.Q<Label>("name");

            BindRows();
        }

        private void BindRows()
        {
            _rows.Add(_tree.Q<VisualElement>("row-one"));
            _rows.Add(_tree.Q<VisualElement>("row-two"));

            _rowNames.Add(_tree.Q<Label>("row-one__name"));
            _rowNames.Add(_tree.Q<Label>("row-two__name"));

            _rowValues.Add(_tree.Q<Label>("row-one__value"));
            _rowValues.Add(_tree.Q<Label>("row-two__value"));
        }

        private void OnDestroy()
        {
            UnsubscribeFromResource();
        }

        public void ShowSelf()
        {
            if (_shown) return;

            _parent.InfoPanel.Add(_tree);
            _shown = true;
        }

        public void HideSelf()
        {
            if (!_shown) return;

            UnsubscribeFromResource();

            _parent.InfoPanel.Remove(_tree);
            _shown = false;
        }

        public void FillIn(Resource resource)
        {
            UnsubscribeFromResource();

            _resource = resource;

            _name.text = $"{resource.Name}";

            ShowRows();

            FillRow(0, $"{_resource.ResourceType}:", $"~{_resource.Quantity}");
            FillRow(1, "Durability:", $"{_resource.Durability}");

            SubscribeToResource();
        }

        private void SubscribeToResource()
        {
            _resource.QuantityChange += UpdateQuantity;
            _resource.DurabilityChange += UpdateDurability;
        }

        private void UnsubscribeFromResource()
        {
            if (_resource != null)
            {
                _resource.QuantityChange -= UpdateQuantity;
                _resource.DurabilityChange -= UpdateDurability;
            }
        }

        private void ShowRows()
        {
            _rows[0].style.display = DisplayStyle.Flex;
            _rows[1].style.display = DisplayStyle.Flex;
        }

        private void UpdateQuantity(int quantity)
        {
            _rowValues[0].text = $"~{quantity}";
        }

        private void UpdateDurability(int durability)
        {
            _rowValues[1].text = $"{durability}";
        }

        private void FillRow(int index, string name, string value)
        {
            _rowNames[index].text = name;
            _rowValues[index].text = value;
        }
    }
}
