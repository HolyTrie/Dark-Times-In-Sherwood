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
        IEnumerator AttackCommitment(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            SetStates(ESP.States.Grounded,ESP.States.Idle);
        }
    }
}