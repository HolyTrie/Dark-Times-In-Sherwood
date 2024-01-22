using UnityEngine;

namespace DTIS
{
    public class AirborneState : PlayerState
    {
        public AirborneState(string name = "Airborne") 
        : base(name,false){}
        public new virtual void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // we do new to preserve the common inherited function
            FSM.Grounded = false;
        }
        protected override void TryStateSwitch()
        {
            if(Controller.Velocity.y == 0)
            {
                SetStates(ESP.States.Grounded,ESP.States.Idle);
            }
        }
        protected override void PhysicsCalculation()
        {

        }
    }
}