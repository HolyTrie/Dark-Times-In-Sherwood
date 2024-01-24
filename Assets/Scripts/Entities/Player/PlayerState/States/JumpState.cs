using UnityEngine;

namespace DTIS
{
    public class JumpState : PlayerState
    {
        public JumpState(string name = "Jump") 
        : base(name){}

        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // critical!
            Controller.Jump();
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
        }
    }
}