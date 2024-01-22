using UnityEngine;

namespace DTIS
{
    public class WalkState:PlayerState {
        public WalkState(string name = "Walk") 
        : base(name) {}
        protected override void TryStateSwitch()
        {
            if(FSM.Controls.HorizontalMove == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
        }
        protected override void PhysicsCalculation()
        {
            Controller.Move(new Vector2(FSM.Controls.HorizontalMove, 0f));
        }
    }
}