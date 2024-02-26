using System;
using UnityEngine;

namespace DTIS
{
    public class RunState:PlayerState {
        private bool IsRunning{get{return Controller.IsRunning;}set{Controller.IsRunning = value;}}
        private bool WasRunning{get{return Controller.WasRunning;}set{Controller.WasRunning = value;}}
        public RunState(ESP.States state,string name = "run") 
        : base(state,name){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            if(Controller.JumpBufferCounter == 0)
                SetAnimations();
            IsRunning = true;
            WasRunning = true;
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
            IsRunning = false;
            if(State != ESP.States.Airborne)
                WasRunning = false;
        }
        protected override void TryStateSwitch()
        {
            if (Controls.WalkingDirection == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            if(ActionMap.Run.WasPerformedThisFrame())
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.WalkingDirection;
            var move =  Controller.RunSpeedMult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}