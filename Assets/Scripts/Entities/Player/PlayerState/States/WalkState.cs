using UnityEngine;

namespace DTIS
{
    public class WalkState : PlayerState
    {
        public WalkState(string name = "Walk")
        : base(name) { }
        protected override void TryStateSwitch()
        {
            if (FSM.Controls.HorizontalMove == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            // if(Controller.Velocity.y == 0)
            // {
            //     SetStates(ESP.States.Fall)
            // }
        }
        protected override void PhysicsCalculation()
        {
            Controller.Physics.Move = new Vector2(FSM.Controls.HorizontalMove, 0f);
        }
    }
}