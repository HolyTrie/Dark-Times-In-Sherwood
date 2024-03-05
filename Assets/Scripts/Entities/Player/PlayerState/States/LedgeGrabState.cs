using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class LedgeGrabState : PlayerState
    {
        private const string GrabLedgeAnimName = "crnr-grb";
        private Vector2 _prevGravity;
        private Collider2D _collider;
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
            _collider = Controller.GetComponent<Collider2D>();
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
            else if (Controls.UpIsPressed && !LeavingLedge)
            {
                FSM.StartCoroutine(ClimbLedge());
            }
        }
        private IEnumerator DropFromLedge()
        {
            Debug.Log("Dropping from ledge");
            LeavingLedge = true;
            var moveY = _collider.bounds.extents.y + 0.1f;
            Vector3 newPos = new(Controller.transform.position.x, Controller.transform.position.y - moveY, Controller.transform.position.z);
            Controller.transform.position = Vector2.Lerp(Controller.transform.position, newPos, 0.85f);
            SetSubState(ESP.States.Fall);
            yield return new WaitForSeconds(.25f);
            LeavingLedge = false;
        }
        private IEnumerator ClimbLedge()
        {
            Debug.Log("climbing ledge");
            Controls.ReadHorizontalInput = false;
            LeavingLedge = true;
            var direction = Controller.FacingRight ? 1f : -1f;
            var moveX = _collider.bounds.extents.x + 0.1f;
            moveX *= direction;
            var moveY = _collider.bounds.size.y + 0.1f;
            Vector3 newPos = new(Controller.transform.position.x + moveX, Controller.transform.position.y + moveY, Controller.transform.position.z);
            Controller.Animator.Play("crnr-clmb");
            Controller.transform.position = Vector2.Lerp(Controller.transform.position, newPos, 0.5f);
            yield return new WaitForSeconds(0.25f);
            newPos.x += direction * 0.5f;
            Controller.transform.position = Vector2.Lerp(Controller.transform.position, newPos, 1.1f);
            Controls.ReadHorizontalInput = true;
            SetSubState(ESP.States.Fall);
            yield return new WaitForSeconds(0.25f);
            LeavingLedge = false;
        }
        protected override void PhysicsCalculation() // is called in FixedUpdate
        {
        }
    }
}