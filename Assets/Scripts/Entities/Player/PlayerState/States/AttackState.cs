using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class AttackState : PlayerState
    {
        public AttackState(string name = "Attack") 
        : base(name,false) 
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
        }
        protected override void TryStateSwitch()
        {
            FSM.StartCoroutine(AttackCommitment(.5f));
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