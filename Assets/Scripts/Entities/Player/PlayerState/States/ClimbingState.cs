using System;
using UnityEngine;

namespace DTIS
{
    public class ClimbingState : PlayerState
    {
        public ClimbingState(ESP.States state,string name = "Climbing") 
        : base(state,name,true){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            SetAnimations();
        }
        protected override void TryStateSwitch()
        {
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}