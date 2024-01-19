using UnityEngine;

namespace DTIS
{
    public class WalkState:EntityState {
        float speedMult;
        public WalkState(string name = "Walk") 
        : base(name) {}
        public override void Exit(EntityController controller)
        {
            // pass
        }
        protected override void TryStateSwitch(EntityStateMachine fsm)
        {

        }
        protected override void PhysicsCalculation(EntityController controller,float Direction)
        {
            controller.Move(new Vector2(Direction, 0f));
        }
    }
}