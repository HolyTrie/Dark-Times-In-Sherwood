using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class AttackState : PlayerState
    {
        private bool isShooting = false;
        public AttackState(ESP.States state,string name = "Attack") 
        : base(state,name,false) 
        {

        }
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // Critical!
            SetAnimations();
            SetSubState(ESP.States.RangedAttack); //for now only this, later we will add diffrenatiaion between attacks
                
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
        }
        protected override void TryStateSwitch()
        {
            //
        }
        protected override void PhysicsCalculation()
        {
           //
        }
    }
}