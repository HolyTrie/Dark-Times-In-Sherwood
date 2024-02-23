using System;
using UnityEngine;

namespace DTIS
{
    public class Jump2State : PlayerState
    {
        private readonly bool _airControl;
        private bool IsInPeakHang{get{return Controller.IsInPeakHang;}set{Controller.IsInPeakHang=value;}}
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
            if(IsInPeakHang)
            {
                IsInPeakHang = false;
            }
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
            if(Mathf.Abs(Controller.Velocity.y) < Controller.JumpPeakHangThreshold && !IsInPeakHang)
            {
                if(!IsInPeakHang) // enter peak hang mode when in threshold 
                {
                    Debug.Log("Jump2 State Entered PeakHang!");
                    IsInPeakHang = true;
                    Controller.CurrGravity *= Controller.JumpPeakGravityMult; 
                }
            }
            if(_airControl)
            {
                Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
            }
        }
    }
}