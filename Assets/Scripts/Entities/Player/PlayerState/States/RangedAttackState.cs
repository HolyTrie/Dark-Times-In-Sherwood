using UnityEngine;

namespace DTIS
{
    public class RangedAttackState : PlayerState
    {
        public RangedAttackState(string name = "RangedAttack") 
        : base(name,true){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // we do new to preserve the common inherited function
            // FSM.groundCheck.Grounded = true;
            Controller.Shoot(); // need to add delay according to frames.
        }
        protected override void TryStateSwitch()
        {
            // Controller.WaitForAnimtaion();
            SetSubState(ESP.States.Idle);
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