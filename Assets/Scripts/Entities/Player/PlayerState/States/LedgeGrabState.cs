using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class LedgeGrabState : PlayerState
    {
        private const string GrabLedgeAnimName = "crnr-grb";
        private const string ClimbLedgeAnimName = "crnr-clmb";
        private bool LeavingLedge { get { return Controller.LeavingLedge; } set { Controller.LeavingLedge = value; } }
        public LedgeGrabState(ESP.States state, string name = "walk")
        : base(state, name, false) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            Debug.Log("entered ledge grab state");
            base.Enter(controller, fsm); // Critical!
            Controller.Animator.Play(GrabLedgeAnimName);
        }
        protected override void TryStateSwitch() // is called in Update
        {
            if (Controls.DownJumpIsPressed && LeavingLedge)
            {
                FSM.StartCoroutine(DropFromLedge());
            }
            else if (Controls.JumpIsPressed && LeavingLedge)
            {
                FSM.StartCoroutine(ClimbLedge());
            }
        }
        private IEnumerator DropFromLedge()
        {
            Debug.Log("Dropping from ledge");
            LeavingLedge = true;
            // TODO: actually move the character appropriately
            yield return new WaitForSeconds(0.25f);
            LeavingLedge = false;
        }
        private IEnumerator ClimbLedge()
        {
            Debug.Log("climbing ledge");
            LeavingLedge = true;
            Controller.Animator.Play(ClimbLedgeAnimName);
            // TODO: actually move the character appropriately
            yield return new WaitForSeconds(0.25f);
            LeavingLedge = false;
        }
        protected override void PhysicsCalculation() // is called in FixedUpdate
        {
        }
    }
}