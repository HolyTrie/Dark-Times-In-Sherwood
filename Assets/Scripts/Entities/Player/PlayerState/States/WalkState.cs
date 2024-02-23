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
            if (Controls.WalkingDirection == 0f && Controls.RunningDirection == 0)
            {
                SetSubState(ESP.States.Idle);
            }
            if(Controls.RunningDirection != 0)
            {
                SetSubState(ESP.States.Run);
            }
        }
        protected override void PhysicsCalculation()
        {
            var direction = Controls.ActionMap.All.Walk.ReadValue<float>();
            var move = new Vector2(direction,0f);
            Controller.Move(move);
        }
    }
}