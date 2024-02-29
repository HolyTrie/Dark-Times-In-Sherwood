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
            if (Controls.WalkingDirection == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            if(Controls.Running)
            {
                SetSubState(ESP.States.Run);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.WalkingDirection;
            var mult = 1f;
            if(Controller.IsInStickyFeet)
            {
                if((Controller.StickyFeetConsidersDirection == false) || ( direction < 0 && Controller.StickyFeetDirectionIsRight) || (direction > 0 && !Controller.StickyFeetDirectionIsRight) )
                {
                    mult *= Controller.StickyFeetFriction;
                }
            }
            var move = new Vector2(mult*direction,0f);
            Controller.Move(move);
        }
    }
}