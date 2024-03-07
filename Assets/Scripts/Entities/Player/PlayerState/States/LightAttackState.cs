using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{

    public class LightAttackState : PlayerState
    {
        private const string Attack1 = "attack1";
        private const string Attack2 = "attack2";
        private const string Attack3 = "attack3";
        private static int attackSequence = 1;
        public LightAttackState(ESP.States state, string name = "attack1")
        : base(state, name, true) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            if (HasAnimation)
            {
                try
                {
                    // controller.Animator.Play(Name);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            if (attackSequence == 1)
            {
                controller.Animator.Play(Attack1);
                attackSequence++;
            }
            else if (attackSequence == 2)
            {
                controller.Animator.Play(Attack2);
                attackSequence++;
            }
            else if (attackSequence == 3)
            {
                controller.Animator.Play(Attack3);
                attackSequence = 1;
            }

            controller.FlipByCursorPos();

            // FSM.StartCoroutine(AttackCommitment(0.55f)); // pretty much wait for animation to finish..
        }

        // IEnumerator AttackCommitment(float seconds)
        // {
        //     // FSM.Controls.enabled = false;
        //     yield return new WaitForSeconds(seconds);
        //     // FSM.Controls.enabled = true;
        //     if (FSM.PrevState.Type == ESP.States.Airborne)
        //         SetStates(ESP.States.Airborne, ESP.States.Fall);
        //     if (FSM.PrevState.Type == ESP.States.Grounded)
        //         SetStates(ESP.States.Grounded, ESP.States.Idle);
        // }

        protected override void PhysicsCalculation()
        {
            // throw new NotImplementedException();
        }

        protected override void TryStateSwitch()
        {

            // Check conditions for transition
            if (!IsAttacking()) // Define IsAttacking() based on your game's logic
            {
                // Transition to another state when not attacking
                if (FSM.PrevState.Type == ESP.States.Airborne)
                    SetStates(ESP.States.Airborne, ESP.States.Fall);
                if (FSM.PrevState.Type == ESP.States.Grounded)
                    SetStates(ESP.States.Grounded, ESP.States.Idle);
            }
        }
        private bool IsAttacking()
        {
            return Controls.ActionMap.All.Shoot.IsPressed();
        }
    }

}