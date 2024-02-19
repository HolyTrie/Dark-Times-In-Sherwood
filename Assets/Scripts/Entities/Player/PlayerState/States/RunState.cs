using UnityEngine;

namespace DTIS
{
    public class RunState:PlayerState {
        public RunState(string name = "Run") 
        : base(name){}
        protected override void TryStateSwitch()
        {
            /*if(!FSM.Controls.Run.triggered)
            {
                
                if(FSM.Controls.HorizontalMove == 0f)
                    FSM.SubState = ESP.Build(ESP.States.Idle);
                else
                    FSM.SubState = ESP.Build(ESP.States.Walk);
                
            }*/
        }
        protected override void PhysicsCalculation()
        {
            //float speed = FSM.Controls.HorizontalMove * Controller.RunSpeedMult;
            //Controller.Move(new Vector2(speed, 0f));
        }
    }
}