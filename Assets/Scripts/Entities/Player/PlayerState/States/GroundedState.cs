using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DTIS
{
    public class GroundedState : PlayerState
    {
        private const float _minYChange = -1.5f;
        private Vector2 SlopeGravity{ get { return Controller.SlopeGravity; } }
        private bool _inSlope = false;
        private bool SlopeAhead { get { return Controller.SlopeAhead; } }
        private Vector2 OriginalGravity{ get { return Controller.OriginalGravity; } }
        public GroundedState(ESP.States state,string name = "Grounded")
        : base(state,name, false) {}
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
            //Debug.Log($"Slope Ahead = {Controller.SlopeAhead}");
            if(Controller.Velocity.y < _minYChange && !Controller.IsGrounded) //some cases like stairs will have negative velocity but are still 'ground'
            {
                SetStates(ESP.States.Airborne, ESP.States.Fall);
            }
            if (ActionMap.Jump.WasPressedThisFrame())
            {
                bool canJump = true;
                if(Controller.StaminaBar != null)
                    if(!Controller.StaminaBar.canUseStamina)
                        canJump = false;
                if(canJump)
                {
                    SetStates(ESP.States.Airborne, ESP.States.Jump);
                }
            }
            if (ActionMap.Shoot.WasPressedThisFrame())
            {   
                SetState(ESP.States.Attack);
            }
        }
        protected override void PhysicsCalculation()
        {
            var playerPos = Controller.Position;
            var hit = Physics2D.Raycast(playerPos,-Vector2.up,1f,Controller.GroundOnlyLayerMask);
            bool slopeUpwardsDirectionIsRight = true;
		    if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f) 
            {
                Vector2 slopeNormal = hit.normal;
                if(slopeNormal.x < 0)
                    slopeUpwardsDirectionIsRight = false;
                //var angle = Mathf.Abs(slopeNormal.x);
                //if(angle < 0 && angle <1)
                //    _onSlope = true;
            }
            if(SlopeAhead && !_inSlope)
            {
                _inSlope = true;
                Controller.CurrGravity = SlopeGravity;
                Debug.Log("increased gravity in anticipation of slope");
            }
            else if(!SlopeAhead && _inSlope)
            {
                FSM.StartCoroutine(LeaveSlope());
            }
            else if(_inSlope) // handle
            {
                if(Controller.FacingRight == slopeUpwardsDirectionIsRight)
                {
                    Controller.CurrGravity = OriginalGravity;
                }
                else
                {
                    Controller.CurrGravity = SlopeGravity;
                }
            }

        }
        private IEnumerator LeaveSlope()
        {
            yield return new WaitForSeconds(0.25f);
            Controller.CurrGravity = OriginalGravity;
            _inSlope = false;
        }
    }
}