﻿using BehaviorDesigner.Runtime.Tasks;
using Units.BehaviorVariables;

namespace Units.BehaviourNodes.Attacking
{
    public class MoveToUnitTarget : Action
    {
        public SharedUnit UnitTarget;

        public UnitMeshAgent UnitMeshAgent;
        public UnitAttacker UnitAttacker;
        public UnitEquipment UnitEquipment;

        private bool OutOfAttackDistance => !UnitAttacker.OnAttackDistance(UnitTarget.Value.transform.position);

        public override void OnStart()
        {
            UnitAttacker.SetTrackedUnit(UnitTarget.Value);
            UnitEquipment.EquipWeapon();
        }

        public override TaskStatus OnUpdate()
        {
            if (!UnitTarget.Value.Alive)
                return TaskStatus.Failure;

            if (!UnitMeshAgent.IsMoving && OutOfAttackDistance)
                UnitMeshAgent.SetDestinationToUnitTarget(UnitTarget.Value, UnitAttacker.AttackDistance);

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            UnitMeshAgent.StopMovingToUnitTarget();
        }
    }
}
