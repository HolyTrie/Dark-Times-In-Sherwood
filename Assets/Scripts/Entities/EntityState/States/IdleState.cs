using UnityEngine;

namespace DTIS
{
    public class IdleState:EntityState {
        public IdleState(string name = "Idle") 
        : base(name){}

        public new void Enter(EntityController controller)
        {
            base.Enter(controller);
            controller.Move(new Vector2(0f,0f));
        }
        public override void Exit(EntityController controller)
        {
            // pass
        }
        protected override void TryStateSwitch(EntityStateMachine fsm)
        {

        }
        protected override void PhysicsCalculation(EntityController controller, float Direction)
        {

        }
    }
}