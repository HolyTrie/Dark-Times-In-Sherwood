using System;
using UnityEngine;

namespace DTIS
{
    public class WalkState : PlayerState
    {
        public WalkState(ESP.States state, string name = "walk")
        : base(state, name, true) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            if (Controller.JumpBufferCounter == 0)
                SetAnimations();
        }
        protected override void TryStateSwitch()
        {
            if (Controls.WalkingDirection == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            if (Controls.RunIsPressed)
            {
                SetSubState(ESP.States.Run);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.WalkingDirection;
            var playerDirection = Controller.FacingRight ? 1f : -1f;
            var mult = 1f;
            if (Controller.IsInStickyFeet)
            {
                if ((Controller.StickyFeetConsidersDirection == false) || (direction < 0 && Controller.StickyFeetDirectionIsRight) || (direction > 0 && !Controller.StickyFeetDirectionIsRight))
                {
                    mult *= Controller.StickyFeetFriction;
                }
            }
            var switchingDirection = (direction > 0 && playerDirection < 0) || ( direction <0 && playerDirection > 0 );
            if(Controls.DownIsPressed && !Controller.EdgeAhead && !switchingDirection)
                mult = 0f;
            var move = mult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}