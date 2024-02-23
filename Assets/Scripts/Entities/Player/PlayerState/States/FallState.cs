using System;
using UnityEngine;

namespace DTIS
{
    public class FallState : PlayerState
    {
        private readonly bool _airControl;
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
            /*
            if(Mathf.Abs(Controller.Velocity.y) < Controller.JumpPeakHangThreshold)
            {
                Controller.CurrGravity *= Controller.JumpPeakGravityMult;
            }
            */
            //else
            //{
                Controller.AccelarateFall();
            //}
            if(_airControl)
            {
                Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
            }
        }
    }
}