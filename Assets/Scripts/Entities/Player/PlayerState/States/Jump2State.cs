using System;
using UnityEngine;

namespace DTIS
{
    public class Jump2State : PlayerState
    {
        private readonly bool _airControl;
        public Jump2State(bool airControl, string name = "smrslt") 
        : base(name)
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
            if(Controller.StaminaBar!= null)
                Controller.StaminaBar.UseStamina(Controller._jumpStaminaCost); // jump co
            Controller.Jump(); //sets jumping to true!
        }
        public override void Exit()
        {
            base.Exit();
            Controller.IsJumping = false;
        }
        protected override void TryStateSwitch() //is called in Update
        {
            if(Controller.Velocity.y < 0 || ActionMap.Jump.WasReleasedThisFrame())
            {
                SetSubState(ESP.States.Fall);
            }
            
        }
        protected override void PhysicsCalculation() // is called in FixedUpdate
        {
            /*
            if(Mathf.Abs(Controller.Velocity.y) < Controller.JumpPeakHangThreshold)
            {
                Controller.CurrGravity *= Controller.JumpPeakGravityMult;
            }
            */
            if(_airControl)
            {
                Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
            }
        }
    }
}