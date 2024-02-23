using System;
using UnityEngine;

namespace DTIS
{
    public class IdleState:PlayerState {
        public IdleState(string name = "idle") 
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
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}