using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class AirborneState : PlayerState
    {
        private readonly int _maxActions = Util.Constants.MaxActionsMidAir;
        private const float _epsilon = 0.001f;
        private int _actionsMidAir = 0;
        private bool _isInCoyoteTime;
        public AirborneState(ESP.States state,string name = "Airborne") 
        : base(state,name,false)
        {
            _isInCoyoteTime = false;
        }
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
            fsm.StartCoroutine(CoyoteTime());
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
        }
        private IEnumerator CoyoteTime()
        {
            _isInCoyoteTime = true;
            yield return new WaitForSeconds(_epsilon+Controller.CoyoteTime); //adding epsilon to account for possible execution lag between Enter() and FixedUpdate().
            _isInCoyoteTime = false;
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
                        if(!_isInCoyoteTime)
                        {
                            ++_actionsMidAir;
                            SetSubState(ESP.States.Jump2);
                        }
                        else
                        {
                            SetSubState(ESP.States.Jump);
                            _isInCoyoteTime = false;
                        }
                    }
                }
                /*TODO: Dash
                else if(ActionMap.Dash.WasPressedThisFrame())
                {
                    ++_actionsMidAir;
                    SetSubState(ESP.States.Dash);
                }
                */ 
            }
            //TODO: add slam (downards jump)
            /*if(ActionMap.DownwardsJump.WasPerformedThisFrame())
            {
                SetSubState(ESP.States.DownwardsJump);
            }
            */
        }   
        protected override void PhysicsCalculation()
        {

        }
    }
}