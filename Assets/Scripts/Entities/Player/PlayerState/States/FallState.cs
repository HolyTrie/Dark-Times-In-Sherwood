using UnityEngine;

namespace DTIS
{
    public class FallState : PlayerState
    {
        public FallState(string name = "Fall")
        : base(name, false)
        {

        }
        protected override void TryStateSwitch()
        {
            FSM.Controller.UpdateGravity = 2f; // if player falls set his mass to bigger just for better smooting

            // if(Controller.Velocity.y == 0)
            // {
            //     SetStates(ESP.States.Grounded,ESP.States.Idle);
            // }
        }
        protected override void PhysicsCalculation()
        {

        }
    }
}