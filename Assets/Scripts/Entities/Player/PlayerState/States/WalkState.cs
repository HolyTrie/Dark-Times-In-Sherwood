using System;
using UnityEngine;

namespace DTIS
{
    public class WalkState : PlayerState
    {
        public WalkState(ESP.States state,string name = "walk")
        : base(state,name,true) { }
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            if(Controller.JumpBufferCounter == 0)
                SetAnimations();
        }
        protected override void TryStateSwitch()
        {
            if (Controls.WalkingDirection == 0f && Controls.RunningDirection == 0)
            {
                SetSubState(ESP.States.Idle);
            }
            if(Controls.RunningDirection != 0)
            {
                SetSubState(ESP.States.Run);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.ActionMap.All.Walk.ReadValue<float>();
            var move = new Vector2(direction,0f);
            Controller.Move(move);
        }
    }
}