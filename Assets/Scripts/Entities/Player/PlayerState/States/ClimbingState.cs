using System;
using UnityEngine;

namespace DTIS
{
    public class ClimbingState : PlayerState
    {
        public ClimbingState(string name = "Climbing") 
        : base(name,false){}
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
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}