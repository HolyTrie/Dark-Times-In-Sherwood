using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class RangedAttackState : PlayerState
    {
        private const string RangedAttackAnim = "RangedAttack";
        private const string AirborneRangedAttackAnim = "AirRangedAttack";

        public RangedAttackState(ESP.States state, string name = "RangedAttack")
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
            if (FSM.PrevState.Type == ESP.States.Airborne)
            {
                FSM.StartCoroutine(GravityWaitsForAttack(0.75f));
                controller.Animator.Play(AirborneRangedAttackAnim);
            }

            if (FSM.PrevState.Type == ESP.States.Grounded)
                controller.Animator.Play(RangedAttackAnim);

            controller.FlipByCursorPos();

            FSM.StartCoroutine(AttackCommitment(1f)); // pretty much wait for animation to finish..
        }
        protected override void TryStateSwitch()
        {

        }
        IEnumerator AttackCommitment(float seconds)
        {
            FSM.Controls.enabled = false;
            yield return new WaitForSeconds(seconds);
            FSM.Controls.enabled = true;
            if (FSM.PrevState.Type == ESP.States.Airborne)
                SetStates(ESP.States.Airborne, ESP.States.Fall);
            if (FSM.PrevState.Type == ESP.States.Grounded)
                SetStates(ESP.States.Grounded, ESP.States.Idle);
        }
        IEnumerator GravityWaitsForAttack(float seconds)
        {
            var prev = Controller.CurrGravity;
            Controller.CurrGravity = new(0f,0f);
            Controller.Velocity = Vector2.zero;
            yield return new WaitForSeconds(seconds);
            Controller.CurrGravity = prev;
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