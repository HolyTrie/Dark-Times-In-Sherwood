using System;
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
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}