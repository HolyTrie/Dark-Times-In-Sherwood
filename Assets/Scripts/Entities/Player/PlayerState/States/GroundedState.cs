using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        public GroundedState(string name = "Grounded") 
        : base(name,false){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // we do new to preserve the common inherited function
            // FSM.groundCheck.Grounded = true;
        }
        protected override void TryStateSwitch()
        {
            if(ActionMap.Jump.WasPressedThisFrame())
            {
                SetStates(ESP.States.Airborne,ESP.States.Jump);
            }
            // if(ActionMap.Walk.IsPressed())
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