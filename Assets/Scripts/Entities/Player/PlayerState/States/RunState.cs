using System;
using UnityEngine;

namespace DTIS
{
    public class RunState:PlayerState {
        private float _prevDirection = 0f;
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
            var direction = Controls.WalkingDirection;
            if (direction == 0f)// && _prevDirection == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            if(!Controls.Running)
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
            _prevDirection = direction;
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.WalkingDirection;
            var move =  Controller.RunSpeedMult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}