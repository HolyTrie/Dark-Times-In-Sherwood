using System;
using UnityEngine;

namespace DTIS
{
    public class WalkState : PlayerState
    {
        public WalkState(string name = "walk")
        : base(name,true) { }
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
            if (FSM.Controls.HorizontalMove == 0f)
            {
                SetSubState(ESP.States.Idle);
            }
        }
        protected override void PhysicsCalculation()
        {
            Controller.Move(new Vector2(FSM.Controls.ActionMap.All.Walk.ReadValue<float>(), 0f));
        }
    }
}