using System;
using UnityEngine;

namespace DTIS
{
    public class JumpState : PlayerState
    {
        private readonly bool _airControl;
        public JumpState(bool airControl, string name = "jump") 
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
            Controller.Jump();
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
            if(_airControl)
            {
                Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
            }
        }
    }
}