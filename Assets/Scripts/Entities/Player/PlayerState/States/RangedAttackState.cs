using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class RangedAttackState : PlayerState
    {
        public RangedAttackState(ESP.States state,string name = "RangedAttack")
        : base(state,name, true) { }
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
            // FSM.groundCheck.Grounded = true;
            // if(controller.isPlaying("HighAttack"))
            Controller.Shoot(); // need to add delay according to frames.
        }
        protected override void TryStateSwitch()
        {
            // Controller.WaitForAnimtaion();
        }

        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
    /*
    public class RangedAttackState : EntityState
    {
        public RangedAttackState(string name = "RangedAttack") 
        : base(name) 
        {

        }
        public override void Enter(EntityController controller)
        {
            // pass
        }
        public override void Exit(EntityController controller)
        {
            // pass
        }
        public override void Update(EntityStateMachine fsm, EntityController controller)
        {
            // pass
        }
        public override void FixedUpdate(EntityStateMachine fsm, EntityController controller)
        {
            // pass
        }
    }
    */
}