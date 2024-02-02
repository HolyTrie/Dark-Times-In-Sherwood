using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        bool isShooting = false;
        public GroundedState(string name = "Grounded") 
        : base(name,false){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // we do new to preserve the common inherited function
            
        }
        protected override void TryStateSwitch()
        {
            
            if(ActionMap.Jump.WasPressedThisFrame())
            {
                SetStates(ESP.States.Airborne,ESP.States.Jump);
            }
            if(ActionMap.Shoot.WasPressedThisFrame() && !isShooting)
            {
                isShooting = true;
                float offset = 3f;
                Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - FSM.Controls.transform.localPosition;
                Debug.Log("Mouse Position: "+ dir + "PlayerPosition: "+ FSM.Controls.transform.localPosition );
                if(dir.y - offset > FSM.Controls.transform.localPosition.y) // aiming above head
                    SetStates(ESP.States.Grounded,ESP.States.HighAttackState);
                else
                    SetStates(ESP.States.Grounded,ESP.States.RangedAttack);
            }
            else
            {
                isShooting = false;
            }
            // else if(ActionMap.Walk.IsPressed())
            // {
                
            //     SetSubState(ESP.States.Walk);
            // }
            /* TODO IF TIME - add falling from platforms behaviour
            if(*Something that makes you fall from a platform [for example]*)
            {
                FSM.Grounded = false;
                SetStates(ESP.States.Airborne,ESP.States.Fall);
            }
            */
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}