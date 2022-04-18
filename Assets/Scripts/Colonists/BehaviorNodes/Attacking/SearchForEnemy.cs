﻿using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Enemies;
using Entities.Ancillaries;
using Entities.BehaviorVariables;
using UnityEngine;

namespace Colonists.BehaviorNodes.Attacking
{
    public class SearchForEnemy : Action
    {
        public FieldOfView FieldOfView;

        public SharedEnemy Enemy;
        public SharedBool NewCommand;

        private float _shortestDistanceToEnemy;

        public override void OnStart()
        {
            Enemy.Value = null;
            _shortestDistanceToEnemy = float.PositiveInfinity;
        }

        public override TaskStatus OnUpdate()
        {
            TryToFind();

            if (Enemy.Value != null)
            {
                NewCommand.Value = true;
            }

            return TaskStatus.Success;
        }

        private void TryToFind()
        {
            foreach (var target in FieldOfView.FindVisibleTargets())
            {
                var enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    SetIfClosest(enemy);
                }
            }
        }

        private void SetIfClosest(Enemy enemy)
        {
            var distanceToEntity = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEntity > _shortestDistanceToEnemy)
            {
                return;
            }

            _shortestDistanceToEnemy = distanceToEntity;
            Enemy.Value = enemy;
        }
    }
}