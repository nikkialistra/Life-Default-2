﻿using System;
using Sirenix.OdinInspector;
using Units.Enums;
using UnityEngine;

namespace Units.Stats
{
    [RequireComponent(typeof(UnitEquipmentStats))]
    public class UnitStats : MonoBehaviour
    {
        [Title("General")]
        [SerializeField] private Stat _movementSpeed;
        [Space]
        [SerializeField] private Stat _maxHealth;
        [SerializeField] private Stat _maxRecoverySpeed;
        [Space]
        [SerializeField] private Stat _dodgeChance;

        [Title("Melee")]
        [SerializeField] private Stat _meleeDamageMultiplier;
        [SerializeField] private Stat _meleeAttackSpeed;
        [SerializeField] private Stat _meleeCriticalChance;
        [SerializeField] private Stat _meleeAttackRange;
        [SerializeField] private Stat _meleeAccuracy;
        
        [Title("Ranged")]
        [SerializeField] private Stat _rangedDamageMultiplier;
        [SerializeField] private Stat _rangedAttackSpeed;
        [SerializeField] private Stat _rangedCriticalChance;
        [SerializeField] private Stat _rangedAttackRange;
        [SerializeField] private Stat _rangedAccuracy;

        private UnitEquipmentStats _unitEquipmentStats;

        public WeaponType WeaponType => _unitEquipmentStats.WeaponType;
        
        public float MeleeCurrentDamage => _unitEquipmentStats.MeleeDamage * _meleeDamageMultiplier.Value;
        public float RangedCurrentDamage => _unitEquipmentStats.RangedDamage * _rangedDamageMultiplier.Value;

        public float Armor => _unitEquipmentStats.Armor;

        public Stat MaxHealth => _maxHealth;
        public Stat MaxRecoverySpeed => _maxRecoverySpeed;
        
        public Stat MovementSpeed => _movementSpeed;
        
        public Stat DodgeChance => _dodgeChance;

        public Stat MeleeDamageMultiplier => _meleeDamageMultiplier;
        public Stat MeleeAttackSpeed => _meleeAttackSpeed;
        public Stat MeleeCriticalChance => _meleeCriticalChance;
        public Stat MeleeAttackRange => _meleeAttackRange;
        public Stat MeleeAccuracy => _meleeAccuracy;

        public Stat RangedDamageMultiplier => _rangedDamageMultiplier;
        public Stat RangedAttackSpeed => _rangedAttackSpeed;
        public Stat RangedCriticalChance => _rangedCriticalChance;
        public Stat RangedAttackRange => _rangedAttackRange;
        public Stat RangedAccuracy => _rangedAccuracy;

        private void Awake()
        {
            _unitEquipmentStats = GetComponent<UnitEquipmentStats>();

            InitializeStats();
        }

        public void AddStatModifier(StatModifier statModifier)
        {
            var stat = ChooseStat(statModifier);

            stat.AddModifier(statModifier);
        }

        public void RemoveStatModifier(StatModifier statModifier)
        {
            var stat = ChooseStat(statModifier);

            stat.RemoveModifier(statModifier);
        }

        private void InitializeStats()
        {
            _movementSpeed.Initialize();
            
            _maxHealth.Initialize();
            _maxRecoverySpeed.Initialize();
            
            _dodgeChance.Initialize();

            _meleeDamageMultiplier.Initialize();
            _meleeAttackSpeed.Initialize();
            _meleeCriticalChance.Initialize();
            _meleeAttackRange.Initialize();
            _meleeAccuracy.Initialize();

            _rangedDamageMultiplier.Initialize();
            _rangedAttackSpeed.Initialize();
            _rangedCriticalChance.Initialize();
            _rangedAttackRange.Initialize();
            _rangedAccuracy.Initialize();
        }

        private Stat ChooseStat(StatModifier statModifier)
        {
            var stat = statModifier.Type switch
            {
                StatType.MovementSpeed => _movementSpeed,
                
                StatType.MaxHealth => _maxHealth,
                StatType.MaxRecoverySpeed => _maxRecoverySpeed,

                StatType.DodgeChance => _dodgeChance,

                StatType.MeleeDamageMultiplier => _meleeDamageMultiplier,
                StatType.MeleeAttackSpeed => _meleeAttackSpeed,
                StatType.MeleeCriticalChance => _meleeCriticalChance,
                StatType.MeleeAttackRange => _meleeAttackRange,
                StatType.MeleeAccuracy => _meleeAccuracy,

                StatType.RangedDamageMultiplier => _rangedDamageMultiplier,
                StatType.RangedAttackSpeed => _rangedAttackSpeed,
                StatType.RangedCriticalChance => _rangedCriticalChance,
                StatType.RangedAttackRange => _rangedAttackRange,
                StatType.RangedAccuracy => _rangedAccuracy,

                _ => throw new ArgumentOutOfRangeException()
            };
            return stat;
        }
    }
}