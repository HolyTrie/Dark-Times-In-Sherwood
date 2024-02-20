using System;
using UnityEngine;

namespace DTIS
{
    public class AirborneState : PlayerState
    {
        private readonly int _maxActions = Util.Constants.MaxActionsMidAir;
        private const float _epsilon = 0.001f;
        private int _actionsMidAir = 0;
        public AirborneState(string name = "Airborne") 
        : base(name,false){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            _actionsMidAir = 1;
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
        }
        protected override void TryStateSwitch()
        {
            if(Controller.IsGrounded)
            {
                SetStates(ESP.States.Grounded,ESP.States.Idle);
            }
            else if(_actionsMidAir < _maxActions)
            {
                
                if(ActionMap.Jump.WasPressedThisFrame())
                {
                    bool canJump = true;
                    if(Controller.StaminaBar!= null)
                        if(Controller.StaminaBar.canUseStamina)
                            canJump = false;
                    if(canJump)
                    {
                        ++_actionsMidAir;
                        SetSubState(ESP.States.Jump2);
                    }
                }
                /*
                else if(ActionMap.Dash.WasPressedThisFrame())
                {
                    ++_actionsMidAir;
                    SetSubState(ESP.States.Dash);
                }
                */ 
            }
        }
        protected override void PhysicsCalculation()
        {

        }
    }
}