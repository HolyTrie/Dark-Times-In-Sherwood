using System;
using UnityEngine;

namespace DTIS
{
    public class IdleState:PlayerState {
        public IdleState(ESP.States state, string name = "idle") 
        : base(state,name){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            if(Controller.JumpBufferCounter == 0)
                SetAnimations();
        }
        protected override void TryStateSwitch()
        {
            
            var direction = Controls.ActionMap.All.Walk.ReadValue<float>();
            if(Controls.ActionMap.All.Run.WasPressedThisFrame() || Controls.ActionMap.All.Run.WasPerformedThisFrame())
            {
                SetSubState(ESP.States.Run);
            }
            else if(direction != 0f)
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}