using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        private const float _minYChange = -1.5f;
        private Vector2 SlopeGravity { get { return Controller.SlopeGravity; } }
        private bool _inSlope = false;
        private bool SlopeAhead { get { return Controller.SlopeAhead; } }
        private Vector2 OriginalGravity { get { return Controller.OriginalGravity; } }
        private Transform crosshair;
        private bool _initialDirectionWasRight;
        public GroundedState(ESP.States state, string name = "Grounded")
        : base(state, name, false) { }
        public override void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            base.Enter(controller, fsm); // Critical!
            Controller.CurrGravity = OriginalGravity;
            if(Controller.JumpBufferCounter > 0)
            {
                Controller.JumpWasBuffered = true;
                SetStates(ESP.States.Airborne,ESP.States.Jump);
                return;
            }
            SetAnimations();
            Controller.JumpBufferCounter = 0f;
            crosshair = controller.transform.Find("Crosshair");
            InitStickyFeet();
        }
        private void InitStickyFeet()
        {
            var hit = Physics2D.Raycast(Controller.Position,-Vector2.up,1f,Controller.WhatIsPlatform);
            if(!hit)
            {
                Debug.Log("no hit for sticky feet");
                Controller.PrevPlatformCollider = null;
                return;
            }
            Collider2D currCollider = hit.collider;
            Debug.Log("found platform collider");

            if(Controller.PrevPlatformCollider != currCollider)
            {
                Controller.PrevPlatformCollider = currCollider;
                Controller.StickyFeetDirectionIsRight = Controller.FacingRight;
                FSM.StartCoroutine(ReleaseStickyFeet());
            }
        }
        protected override void TryStateSwitch()
        {
            if (Controller.Velocity.y < _minYChange && !Controller.IsGrounded) //some cases like stairs will have negative velocity but are still 'ground'
            {
                SetStates(ESP.States.Airborne, ESP.States.Fall);
            }
            if (ActionMap.Jump.WasPressedThisFrame())
            {
                bool canJump = true;
                if (canJump)
                {
                    SetStates(ESP.States.Airborne, ESP.States.Jump);
                }
            }
            if (ActionMap.Shoot.WasPressedThisFrame())
            {
                SetState(ESP.States.Attack);
            }

            //swap weapons//

            if (ActionMap.SwapWeapon.WasPressedThisFrame())
            {
                //TODO: enum instead of nums
                Debug.Log("Swapping Weapons");
                AttackState.weaponType++;
                if (AttackState.weaponType > 2)
                    AttackState.weaponType = 1;
            }

            //handles the view of crosshair//

            if (AttackState.weaponType == 1) // sword
            {
                crosshair.gameObject.SetActive(false);
            }
            if (AttackState.weaponType == 2) // bow
            {
                crosshair.gameObject.SetActive(true);
            }
        }
        protected override void PhysicsCalculation()
        {
            HandleSlopes();
        }
        private void HandleSlopes()
        {
            var playerPos = Controller.Position;
            var hit = Physics2D.Raycast(playerPos, -Vector2.up, 1f, Controller.GroundOnlyLayerMask);
            bool slopeUpwardsDirectionIsRight = true;
            if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
            {
                Vector2 slopeNormal = hit.normal;
                if (slopeNormal.x > 0)
                    slopeUpwardsDirectionIsRight = false;
                var angle = Mathf.Abs(slopeNormal.x);
                if (angle < 0 && angle < 1)
                    _inSlope = true;
            }
            bool dirIsRight = slopeUpwardsDirectionIsRight;
            if (!SlopeAhead && _inSlope)
            {
                FSM.StartCoroutine(LeaveSlope());
            }
            else if (_inSlope || (SlopeAhead && !_inSlope))
            {
                SetSlopeGravity(dirIsRight);
            }
        }
        private void SetSlopeGravity(bool slopeUpwardsDirectionIsRight)
        {
            if (Controller.FacingRight == slopeUpwardsDirectionIsRight)
            {
                Controller.CurrGravity = 2f * OriginalGravity;
            }
            else
            {
                Controller.CurrGravity = 5f * SlopeGravity;
            }
        }
        private IEnumerator LeaveSlope()
        {
            yield return new WaitForSeconds(0.25f);
            Controller.CurrGravity = OriginalGravity;
        }

        private IEnumerator ReleaseStickyFeet()
        { 
            Controller.IsInStickyFeet = true;
            yield return new WaitForSeconds(Controller.StickyFeetDuration);
            Controller.IsInStickyFeet = false;
        }
    }
}