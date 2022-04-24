﻿using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Enemies;
using Entities;
using Entities.Types;
using ResourceManagement;
using Units.BehaviorVariables;
using UnityEngine;

namespace Colonists
{
    [RequireComponent(typeof(ColonistMeshAgent))]
    [RequireComponent(typeof(BehaviorTree))]
    public class ColonistBehavior : MonoBehaviour
    {
        private BehaviorTree _behaviorTree;

        private ColonistMeshAgent _colonistMeshAgent;

        private SharedPositions _positions;
        private SharedFloat _rotation;
        private SharedColonist _colonist;
        private SharedEnemy _enemy;
        private SharedResource _resource;
        private SharedUnit _unitTarget;

        private SharedBool _newCommand;

        private bool _initialized;

        private void Awake()
        {
            _colonistMeshAgent = GetComponent<ColonistMeshAgent>();
            _behaviorTree = GetComponent<BehaviorTree>();
        }

        public void Activate()
        {
            if (!_initialized)
            {
                Initialize();
            }

            _behaviorTree.EnableBehavior();
        }

        public void Deactivate()
        {
            _behaviorTree.DisableBehavior();
        }

        public void Stop()
        {
            ResetParameters();
            _newCommand.Value = true;
        }

        public void OrderTo(Colonist targetColonist)
        {
            if (!_colonistMeshAgent.CanAcceptOrder())
            {
                return;
            }
            
            ResetParameters();
            _colonist.Value = targetColonist;

            _newCommand.Value = true;
        }

        public void OrderTo(Enemy enemy)
        {
            if (!_colonistMeshAgent.CanAcceptOrder())
            {
                return;
            }
            
            ResetParameters();
            _enemy.Value = enemy;

            _newCommand.Value = true;
        }

        public void OrderTo(Resource resource)
        {
            if (!_colonistMeshAgent.CanAcceptOrder())
            {
                return;
            }

            ResetParameters();
            _resource.Value = resource;

            _newCommand.Value = true;
        }

        private void ResetParameters()
        {
            _positions.Value.Clear();
            _rotation.Value = float.NegativeInfinity;
            
            _colonist.Value = null;
            _enemy.Value = null;
            _resource.Value = null;
            
            _unitTarget.Value = null;
        }

        public void OrderToPosition(Vector3 position, float? angle)
        {
            if (!_colonistMeshAgent.CanAcceptOrder())
            {
                return;
            }

            ResetParameters();
            _positions.Value.Enqueue(position);
            if (angle.HasValue)
            {
                _rotation.Value = angle.Value;
            }

            _newCommand.Value = true;
        }

        public void AddPositionToOrder(Vector3 position, float? angle)
        {
            if (!_colonistMeshAgent.CanAcceptOrder())
            {
                return;
            }
            
            if (_positions.Value.Count == 0)
            {
                OrderToPosition(position, angle);
            }

            _positions.Value.Enqueue(position);
            if (angle.HasValue)
            {
                _rotation.Value = angle.Value;
            }
        }

        private void Initialize()
        {
            _newCommand = (SharedBool)_behaviorTree.GetVariable("NewCommand");

            _positions = (SharedPositions)_behaviorTree.GetVariable("Positions");
            _rotation = (SharedFloat)_behaviorTree.GetVariable("Rotation");

            _colonist = (SharedColonist)_behaviorTree.GetVariable("Colonist");
            _enemy = (SharedEnemy)_behaviorTree.GetVariable("Enemy");
            _resource = (SharedResource)_behaviorTree.GetVariable("Resource");
            _unitTarget = (SharedUnit)_behaviorTree.GetVariable("UnitTarget");

            _positions.Value = new Queue<Vector3>();

            _initialized = true;
        }
    }
}
