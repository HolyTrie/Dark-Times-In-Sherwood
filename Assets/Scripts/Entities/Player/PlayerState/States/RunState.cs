using System;
using UnityEngine;

namespace DTIS
{
    public class RunState:PlayerState {
        public RunState(string name = "run") 
        : base(name){}
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
            controller.IsRunning = true;
        }
        public override void Exit()
        {
            base.Exit();
            Controller.IsRunning = false;
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