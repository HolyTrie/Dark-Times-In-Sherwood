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