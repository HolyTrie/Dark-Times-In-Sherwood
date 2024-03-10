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
        private static bool _attackCommit = false;
        public LightAttackState(ESP.States state, string name = "attack1")
        : base(state, name, true) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            _attackCommit = true;
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
            Debug.Log("ATTACK SEQUENCE: " + attackSequence);
            if (attackSequence == 1)
                controller.Animator.Play(Attack1);
            else if (attackSequence == 2)
                controller.Animator.Play(Attack2);
            else if (attackSequence == 3)
            {
                controller.Animator.Play(Attack3);
                attackSequence = 1; // resets everytime
            }
            else
            {
                attackSequence = 1; // resets everytime
            }

            controller.FlipByCursorPos();
            
            
            FSM.StartCoroutine(AttackCommitment(0.55f)); // pretty much wait for animation to finish..
        }

        IEnumerator AttackCommitment(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            if (FSM.PrevState.Type == ESP.States.Airborne)
                SetStates(ESP.States.Airborne, ESP.States.Fall);
            if (FSM.PrevState.Type == ESP.States.Grounded)
                SetStates(ESP.States.Grounded, ESP.States.Idle);

            if (IsAttackingWasPressed()) // if attack is pressed once
            {
                attackSequence++;
                SetSubState(ESP.States.LightAttack);
            }
            else if (IsAttackingHold()) // if player holds the attack it wil enter attack sequence.
            {
                attackSequence++;
                SetSubState(ESP.States.LightAttack);
            }
            _attackCommit = false;
        }

        protected override void PhysicsCalculation()
        {
            //allows movement while attacking//
            var direction = Controls.WalkingDirection;
            var mult = 0.5f;
            var move = mult * new Vector2(direction, 0f);
            Controller.Move(move);
        }

        protected override void TryStateSwitch()
        {
            // If the shoot button is released and not wating for attack to finish, transition back to idle
            if (!IsAttackingHold() && !_attackCommit)
                SetStates(ESP.States.Grounded, ESP.States.Idle);
        }
        private bool IsAttackingHold()
        {
            return Controls.ActionMap.All.Shoot.IsPressed();
        }

        private bool IsAttackingWasPressed()
        {
            return Controls.ActionMap.All.Shoot.WasPressedThisFrame();
        }
    }
}