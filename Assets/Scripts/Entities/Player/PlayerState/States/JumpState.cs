using System;
using UnityEngine;

namespace DTIS
{
    public class JumpState : PlayerState
    {
        private readonly bool _airControl;
        private bool IsInPeakHang{get{return Controller.IsInPeakHang;}set{Controller.IsInPeakHang=value;}}
        private bool WasRunning{get{return Controller.WasRunning;}set{Controller.WasRunning=value;}}
        public JumpState(ESP.States state,bool airControl, string name = "jump") 
        : base(state,name)
        {
           _airControl = airControl;
        }
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            Debug.Log("entered jump");
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
                Controller.StaminaBar.UseStamina(Controller._jumpStaminaCost); // jump cost
            if(IsInPeakHang)
            {
                IsInPeakHang = false;
            }
            Controller.Jump(); //sets jumping to true!
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
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
                    IsInPeakHang = true;
                    Controller.CurrGravity *= Controller.JumpPeakGravityMult; 
                }
            }
            if(_airControl)
            { 
                var direction = FSM.Controls.ActionMap.All.Walk.ReadValue<float>();
                float mult = 1.0f;
                if(IsInPeakHang)
                {
                    mult *= 0.5f;
                }
                if(WasRunning)
                {
                    mult *= Controller.RunSpeedMult;
                }
                Controller.Move(new Vector2(mult*direction, 0f));
            }
        }
    }
}