using System;
using UnityEngine;

namespace DTIS
{
    public class RunState : PlayerState
    {
        //TODO: merge with Walk state!!! they are too similar.
        private bool IsRunning { /*get { return Controller.IsRunning; }*/ set { Controller.IsRunning = value; } }
        private bool WasRunning { /*get { return Controller.WasRunning; }*/ set { Controller.WasRunning = value; } }
        public RunState(ESP.States state, string name = "run")
        : base(state, name) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            if (Controller.JumpBufferCounter == 0)
                SetAnimations();
            IsRunning = true;
            WasRunning = true;
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
            IsRunning = false;
            if (State != ESP.States.Airborne)
                WasRunning = false;
        }
        protected override void TryStateSwitch()
        {
            var direction = Controls.WalkingDirection;
            if (direction == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            if (!Controls.RunIsPressed)
            {
                SetSubState(ESP.States.Walk);
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
            mult *= Controller.RunSpeedMult;
            var switchingDirection = (direction > 0 && playerDirection < 0) || ( direction <0 && playerDirection > 0 );
            if(Controls.DownIsPressed && !Controller.EdgeAhead && !switchingDirection)
                mult = 0f;
            var move = mult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}