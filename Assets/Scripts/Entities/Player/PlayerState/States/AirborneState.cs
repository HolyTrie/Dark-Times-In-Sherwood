using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class AirborneState : PlayerState
    {
        private readonly int _maxJumps = 2;
        private readonly int _maxAttacks = 1; //bow/slam
        private const float _epsilon = 0.001f;
        private int _jumpsMidAir = 0;
        private int _attacksMidAir = 0;
        private bool _isInCoyoteTime = false;
        public AirborneState(ESP.States state, string name = "Airborne")
        : base(state, name, false) {}
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            SetAnimations();
            _jumpsMidAir = 1; //this is 1 to account for the jump that initiated this state (or missed jump when falling from platforms)
            _attacksMidAir = 0;
            fsm.StartCoroutine(CoyoteTime());
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
        }
        private IEnumerator CoyoteTime()
        {
            _isInCoyoteTime = true;
            yield return new WaitForSeconds(_epsilon + Controller.CoyoteTime); //adding epsilon to account for possible execution lag between Enter() and FixedUpdate().
            _isInCoyoteTime = false;
        }

        protected override void TryStateSwitch()
        {
            bool jumpPressedThisFrame = ActionMap.Jump.WasPressedThisFrame();
            if(jumpPressedThisFrame)
                Controller.JumpBufferCounter = Controller.JumpBufferTime;
            else
                Controller.JumpBufferCounter -= Time.deltaTime;
            if (Controller.IsGrounded)
            {
                SetStates(ESP.States.Grounded, ESP.States.Idle);
            }
            else if (_jumpsMidAir < _maxJumps && jumpPressedThisFrame)
            {
                bool canJump = true;
                /*
                if (Controller.StaminaBar != null)
                    if (Controller.StaminaBar.canUseStamina)
                        canJump = false;
                */
                if (canJump)
                {
                    if (!_isInCoyoteTime)
                    {
                        ++_jumpsMidAir;
                        SetSubState(ESP.States.Jump);
                    }
                    else
                    {
                        SetSubState(ESP.States.Jump);
                        _isInCoyoteTime = false;
                    }
                    Controller.JumpBufferCounter = 0f;
                }
                
            }
            if (_attacksMidAir < _maxAttacks)
            {
                if (ActionMap.Shoot.WasPressedThisFrame())
                {
                    ++_attacksMidAir;
                    SetState(ESP.States.Attack);
                }
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