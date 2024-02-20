using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    public class FallState : PlayerState
    {
        private readonly bool _airControl;
        public FallState(bool airControl,string name = "fall")
        : base(name, false)
        {
            _airControl = airControl;
        }
        protected override void TryStateSwitch()
        {
            // TODO: control jump velocities VVVVV          like so VVVV
            //FSM.Controller.UpdateGravity = 2f; // if player falls set his mass to bigger just for better smooting

            // if(Controller.Velocity.y == 0)
            // {
            //     SetStates(ESP.States.Grounded,ESP.States.Idle);
            // }
        }
        protected override void PhysicsCalculation()
        {
            if(_airControl)
            {
                Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
            }
            if(Controller.Velocity.y > 0)
            {
                Controller.DeaccelarateJump();
            }
        }
    }
}