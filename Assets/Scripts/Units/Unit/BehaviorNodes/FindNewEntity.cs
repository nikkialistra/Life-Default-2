﻿using NPBehave;

namespace Units.Unit.BehaviorNodes
{
    public class FindNewEntity : Node
    {
        public FindNewEntity() : base("FindNewTarget")
        {
            
        }
        
        protected override void DoStart()
        {
            Stopped(false);
        }

        protected override void DoStop()
        {
            Stopped(false);
        }
    }
}