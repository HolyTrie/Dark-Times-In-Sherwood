using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DTIS
{
    public class AttackState : PlayerState
    {
        public static int weaponType = 1;
        public AttackState(ESP.States state, string name = "Attack")
        : base(state, name, false)
        {

        }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            SetAnimations();
            if (weaponType == 1) // sword
            {
                SetSubState(ESP.States.LightAttack);
            }
            if (weaponType == 2) // bow
            {
                SetSubState(ESP.States.RangedAttack);
            }

        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
        }
        protected override void TryStateSwitch()
        {
            
        }
        protected override void PhysicsCalculation()
        {
            //
        }
    }
}