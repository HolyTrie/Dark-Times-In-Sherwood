using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        bool isShooting = false;
        private const float _minYChange = -1.5f;

        public GroundedState(ESP.States state,string name = "Grounded")
        : base(state,name, false) { }
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
        }
        protected override void TryStateSwitch()
        {
            //Debug.Log($"Slope Ahead = {Controller.SlopeAhead}");
            if(Controller.Velocity.y < _minYChange && !Controller.IsGrounded) //some cases like stairs will have negative velocity but are still 'ground'
            {
                SetStates(ESP.States.Airborne, ESP.States.Fall);
            }
            if (ActionMap.Jump.WasPressedThisFrame())
            {
                bool canJump = true;
                if(Controller.StaminaBar != null)
                    if(!Controller.StaminaBar.canUseStamina)
                        canJump = false;
                if(canJump)
                {
                    SetStates(ESP.States.Airborne, ESP.States.Jump);
                }
            }
            if (ActionMap.Shoot.WasPerformedThisFrame() && !isShooting)
            {
                isShooting = true;
                float offset = 3f;
                Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Controller.transform.localPosition;
                //Debug.Log("Mouse Position: "+ dir + "PlayerPosition: "+ FSM.Controls.transform.localPosition );
                if (dir.y - offset > Controller.transform.localPosition.y) // aiming above head
                {
                    //if (!Controller.isPlaying("HighAttack"))
                    SetStates(ESP.States.Attack, ESP.States.HighAttackState);
                }
                else
                {
                    //if (!Controller.isPlaying("RangedAttack"))
                    SetStates(ESP.States.Attack, ESP.States.RangedAttack);
                }
            }
            else
            {
                isShooting = false;
            }
        }
        private bool _inSlope = false;
        private bool SlopeAhead { get { return Controller.SlopeAhead; } }
        private Vector2 _prevGravity;
        protected override void PhysicsCalculation()
        {
            if(SlopeAhead && !_inSlope)
            {
                _inSlope = true;
                _prevGravity = Controller.CurrGravity;
                Controller.CurrGravity *= 2f;
            }
            if(!SlopeAhead && _inSlope)
            {
                
                FSM.StartCoroutine(LeaveSlope());
            }
        }

        private IEnumerator LeaveSlope()
        {
            yield return new WaitForSeconds(0.25f);
            Controller.CurrGravity = _prevGravity;
            _inSlope = true;
        }
    }
}