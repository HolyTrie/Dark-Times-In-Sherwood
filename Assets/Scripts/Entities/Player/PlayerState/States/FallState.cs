using System;
using UnityEngine;

namespace DTIS
{
    public class FallState : PlayerState
    {
        private readonly bool _airControl;
        private bool IsInPeakHang{get{return Controller.IsInPeakHang;}set{Controller.IsInPeakHang=value;}}

        public FallState(bool airControl,string name = "fall")
        : base(name, true)
        {
            _airControl = airControl;
        }
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
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
            Controller.IsFalling = true;
        }
        public override void Exit()
        {
            Controller.IsFalling = false;
        }
        protected override void TryStateSwitch()
        {
            //pass
        }
        protected override void PhysicsCalculation()
        {
            if(Mathf.Abs(Controller.Velocity.y) >= Controller.JumpPeakHangThreshold && IsInPeakHang)
            {
                Debug.Log("Fall exits from peak hang mode");
                IsInPeakHang = false;
                Controller.CurrGravity = Controller.FallGravity;
            }
            if(_airControl)
            {
                var direction = FSM.Controls.ActionMap.All.Walk.ReadValue<float>();
                float mult = 1.0f;
                if(IsInPeakHang)
                {
                    mult = 0.5f;
                }
                Controller.Move(new Vector2(mult*direction, 0f));
            }
        }
    }
}