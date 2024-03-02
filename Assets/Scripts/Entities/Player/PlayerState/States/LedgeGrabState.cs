using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class LedgeGrabState : PlayerState
    {
        private const string GrabLedgeAnimName = "crnr-grb";
        private Vector2 _prevGravity;
        private bool LeavingLedge { get { return Controller.LeavingLedge; } set { Controller.LeavingLedge = value; } }
        public LedgeGrabState(ESP.States state, string name = "walk")
        : base(state, name, false) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            Debug.Log("entered ledge grab state");
            base.Enter(controller, fsm); // Critical!
            Controller.Animator.Play(GrabLedgeAnimName);
            Controller.Velocity = Vector2.zero;
            _prevGravity = Controller.CurrGravity;
            Controller.CurrGravity = Vector2.zero;
            Controller.GrabbingLedge = true;
        }
        public override void Exit(ESP.States State, ESP.States SubState)
        {
            base.Exit(State, SubState);
            Controller.CurrGravity = _prevGravity;
            FSM.StartCoroutine(ReleaseGrabParam());
        }
        private IEnumerator ReleaseGrabParam()
        {
            yield return new WaitForSeconds(.25f);
            Controller.GrabbingLedge = false;
        }
        protected override void TryStateSwitch() // is called in Update
        {
            if (Controls.DownIsPressed && !LeavingLedge)
            {
                FSM.StartCoroutine(DropFromLedge());
            }
            else if (Controls.JumpIsPressed && !LeavingLedge)
            {
                FSM.StartCoroutine(ClimbLedge());
            }
        }
        private IEnumerator DropFromLedge()
        {
            Debug.Log("Dropping from ledge");
            LeavingLedge = true;
            SetSubState(ESP.States.Fall);
            yield return new WaitForSeconds(.25f);
            LeavingLedge = false;
        }
        private IEnumerator ClimbLedge()
        {
            Debug.Log("climbing ledge");
            LeavingLedge = true;
            SetSubState(ESP.States.Jump);
            yield return new WaitForSeconds(0.25f);
            LeavingLedge = false;
        }
        protected override void PhysicsCalculation() // is called in FixedUpdate
        {
        }
    }
}