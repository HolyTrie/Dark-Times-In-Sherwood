using Unity.VisualScripting;
using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        bool isShooting = false;
        public GroundedState(string name = "Grounded")
        : base(name, false) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm);
        }
        protected override void TryStateSwitch()
        {
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
            if (ActionMap.Shoot.IsPressed() && !isShooting)
            {
                isShooting = true;
                float offset = 3f;
                Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - FSM.Controls.transform.localPosition;
                //Debug.Log("Mouse Position: "+ dir + "PlayerPosition: "+ FSM.Controls.transform.localPosition );
                if (dir.y - offset > FSM.Controls.transform.localPosition.y) // aiming above head
                {
                    if (!Controller.isPlaying("HighAttack"))
                        SetStates(ESP.States.Grounded, ESP.States.HighAttackState); // TODO: MAIN STATE - ATTACK
                }
                else if (dir.y - offset <= FSM.Controls.transform.localPosition.y)
                {
                    if (!Controller.isPlaying("RangedAttack"))
                        SetStates(ESP.States.Grounded, ESP.States.RangedAttack); // TODO: MAIN STATE - ATTACK
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