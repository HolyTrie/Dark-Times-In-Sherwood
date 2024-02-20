using System;
using UnityEngine;

namespace DTIS
{
    public class HighAttackState : PlayerState
    {
        public HighAttackState(string name = "HighAttack") 
        : base(name,true){}
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
            //SetSubState(ESP.States.Idle);
            // else if(ActionMap.Walk.IsPressed())
            // {
                
            //     SetSubState(ESP.States.Walk);
            // }
            /* TODO IF TIME - add falling from platforms behaviour
            if(*Something that makes you fall from a platform [for example]*)
            {
                FSM.Grounded = false;
                SetStates(ESP.States.Airborne,ESP.States.Fall);
            }
            */
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