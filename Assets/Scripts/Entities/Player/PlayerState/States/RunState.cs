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
        }
        protected override void TryStateSwitch()
        {
            if(!Controls.ActionMap.All.Run.WasPerformedThisFrame())
            {
                FSM.SubState = ESP.Build(ESP.States.Walk);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.ActionMap.All.Run.ReadValue<float>();
            var move =  Controller.RunSpeedMult * new Vector2(direction, 0f);
            Controller.Move(move);
        }
    }
}