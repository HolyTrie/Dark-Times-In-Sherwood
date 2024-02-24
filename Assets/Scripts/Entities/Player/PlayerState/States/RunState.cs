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
            if (HasAnimation)
            {
                try
                {
                    controller.Animator.Play(Name);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
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
            /*
            if (FSM.Controls.HorizontalMove == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
            */
            if(Controls.RunningDirection == 0)
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.ActionMap.All.Run.ReadValue<float>(); //using All.Walk is the same btw
            var move =  Controller.RunSpeedMult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}