﻿using System;
using System.Collections;
using Colonists;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(UnitStats))]
    [RequireComponent(typeof(UnitAnimator))]
    public class UnitAttacker : MonoBehaviour
    {
        [Required]
        [SerializeField] private UnitEquipment _unitEquipment;
        [Space]
        [SerializeField] private float _waitTime = 0.2f;

        private Unit _unit;
        
        private Action _onInteractionFinish;

        private UnitStats _unitStats;
        
        private UnitAnimator _unitAnimator;

        private Coroutine _attackingCoroutine;

        private void Awake()
        {
            _unitStats = GetComponent<UnitStats>();
            _unitAnimator = GetComponent<UnitAnimator>();
        }

        public float AttackRange => _unitStats.MeleeAttackRange;

        public void Attack(Unit unit, Action onInteractionFinish)
        {
            _unit = unit;
            _onInteractionFinish = onInteractionFinish;

            _unitEquipment.EquipWeapon();
            _unitAnimator.Attack(true);

            _attackingCoroutine = StartCoroutine(WatchForDestroy());
        }

        public void Hit(float passedTime)
        {
            if (_unit == null || !_unit.Alive)
            {
                FinishAttacking();
                return;
            }

            var damage = _unitStats.MeleeDamagePerSecond * passedTime;

            _unit.TakeDamage(damage);

            if (!_unit.Alive)
            {
                _unit = null;
                FinishAttacking();
            }
        }

        public bool OnAttackRange(Vector3 position)
        {
            return Vector3.Distance(transform.position, position) < _unitStats.MeleeAttackRange;
        }

        private IEnumerator WatchForDestroy()
        {
            while (_unit.Alive)
            {
                yield return new WaitForSeconds(_waitTime);
            }

            _unit = null;

            FinishAttacking();
        }

        public void StopAttacking()
        {
            if (_attackingCoroutine != null)
            {
                StopCoroutine(_attackingCoroutine);
                _attackingCoroutine = null;
            }

            _unit = null;

            StartCoroutine(StopAttackingLater());
        }

        private IEnumerator StopAttackingLater()
        {
            yield return new WaitForSeconds(_waitTime);

            FinishAttacking();
        }
        
        private void FinishAttacking()
        {
            if (_unit != null)
            {
                return;
            }

            if (_attackingCoroutine != null)
            {
                StopCoroutine(_attackingCoroutine);
                _attackingCoroutine = null;
            }
            
            _unitAnimator.Attack(false);

            if (_onInteractionFinish != null)
            {
                _onInteractionFinish();
                _onInteractionFinish = null;
            }
        }
    }
}