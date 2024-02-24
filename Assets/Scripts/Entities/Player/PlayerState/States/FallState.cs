using System;
using UnityEngine;

namespace DTIS
{
    public class FallState : PlayerState
    {
        private readonly bool _airControl;
        private bool IsInPeakHang { get { return Controller.IsInPeakHang; } set { Controller.IsInPeakHang = value; } }
        private bool WasRunning { get { return Controller.WasRunning; } set { Controller.WasRunning = value; } }
        private bool IsFalling { get { return Controller.IsFalling; } set { Controller.IsFalling = value; } }

        public FallState(ESP.States state,bool airControl, string name = "fall")
        : base(state, name, true)
        {
            _airControl = airControl;
        }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            if (HasAnimation)
            {
                try
                {
                    controller.Animator.Play(Name);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            IsFalling = true;
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
            IsFalling = false;
            IsInPeakHang = false;
            WasRunning = false;
        }
        protected override void TryStateSwitch()
        {
            //pass
        }
        protected override void PhysicsCalculation()
        {
            bool inPeakHangThreshold = Mathf.Abs(Controller.Velocity.y) < Controller.JumpPeakHangThreshold;
            if (inPeakHangThreshold && !IsInPeakHang) // happens when button is released early.
            {
                IsInPeakHang = true;
                Controller.CurrGravity *= Controller.JumpPeakGravityMult;
            }
            if (!inPeakHangThreshold)
            {
                IsInPeakHang = false;
                Controller.CurrGravity = Controller.FallGravity;
            }
            if (_airControl)
            {
                var direction = FSM.Controls.ActionMap.All.Walk.ReadValue<float>();
                float mult = 1.0f;
                if (IsInPeakHang)
                {
                    mult *= 0.5f;
                }
                if (WasRunning)
                {
                    mult *= Controller.RunSpeedMult;
                }
                Controller.Move(new Vector2(mult * direction, 0f));
            }
        }
    }
}