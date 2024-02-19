using UnityEngine;

namespace DTIS
{
    public class JumpState : PlayerState
    {
        private readonly bool _airControl;
        public JumpState(bool airControl, string name = "Jump") 
        : base(name)
        {
           _airControl = airControl;
        }

        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // critical!
            Controller.StaminaBar.UseStamina(Controller._jumpStaminaCost); // jump co
            Controller.Physics.Jump();
        }
        protected override void TryStateSwitch()
        {
            if(Controller.Velocity.y < 0)
            {
                SetSubState(ESP.States.Fall);
            }
        }
        protected override void PhysicsCalculation()
        {
            if(_airControl)
            {
                Controller.Physics.Move = new Vector2(FSM.Controls.HorizontalMove, 0f);
            }
        }
    }
}