﻿using System;
using Game.UI.Game;
using Kernel.Entities;
using Kernel.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Units.Unit
{
    [RequireComponent(typeof(UnitSaveLoadHandler))]
    [RequireComponent(typeof(EntityHealth))]
    public class UnitFacade : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
    {
        [Title("Properties")]
        [SerializeField] private UnitType _unitType;
        [SerializeField] private string _name;

        [Title("Indicators")]
        [Required] 
        [SerializeField] private HealthIndicatorView _healthIndicatorView;
        [Required]
        [SerializeField] private GameObject _selectionIndicator;

        public UnitSaveLoadHandler UnitSaveLoadHandler { get; private set;  }

        public UnitType UnitType => _unitType;
        public string Name => _name;
        public int Health => _health.Health;
        
        private EntityHealth _health;

        private IMemoryPool _pool;

        private void Awake()
        {
            _health = GetComponent<EntityHealth>();
            UnitSaveLoadHandler = GetComponent<UnitSaveLoadHandler>();
        }

        private void OnEnable()
        {
            _health.Died += Dispose;
            _health.HealthChange += OnHealthChange;
        }

        private void OnDisable()
        {
            _health.Died -= Dispose;
            _health.HealthChange -= OnHealthChange;
        }

        private void Start()
        {
            if (_name == "")
            {
                _name = NameGenerator.GetRandomName();
            }

            _healthIndicatorView.SetHealth(_health.Health);
        }

        [Button(ButtonSizes.Large)]
        public void TakeDamage(int value)
        {
            _health.TakeDamage(value);
        }

        public void Select()
        {
            _selectionIndicator.SetActive(true);
            _healthIndicatorView.Show();
        }

        public void Deselect()
        {
            _selectionIndicator.SetActive(false);
            _healthIndicatorView.Hide();
        }

        public void OnSpawned(Vector3 position, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void Dispose()
        {
            if (_pool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                _pool.Despawn(this);
            }
        }

        private void OnHealthChange(int value)
        {
            _healthIndicatorView.SetHealth(value);
        }

        public class Factory : PlaceholderFactory<Vector3, UnitFacade>
        {
        }
    }
}