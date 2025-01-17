﻿using System;
using BehaviorDesigner.Runtime;

namespace Units.BehaviorVariables
{
    [Serializable]
    public class SharedUnit : SharedVariable<Unit>
    {
        public static implicit operator SharedUnit(Unit value)
        {
            return new SharedUnit { Value = value };
        }
    }
}
