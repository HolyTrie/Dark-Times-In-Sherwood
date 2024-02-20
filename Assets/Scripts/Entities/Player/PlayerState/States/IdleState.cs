using UnityEngine;

namespace DTIS
{
    public class IdleState:PlayerState {
        public IdleState(string name = "idle") 
        : base(name){}
        protected override void TryStateSwitch()
        {
            if(FSM.Controls.HorizontalMove != 0f)
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}