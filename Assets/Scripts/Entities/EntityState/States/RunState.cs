using UnityEngine;

namespace DTIS
{
    public class RunState:EntityState {
        public RunState(string name = "Run") 
        : base(name) 
        {
            
        }
        public override void Exit(EntityController controller)
        {
            // pass
        }
        protected override void TryStateSwitch(EntityStateMachine fsm)
        {

        }
        protected override void PhysicsCalculation(EntityController controller,float Direction)
        {
            controller.Move(new Vector2(Direction * controller.RunSpeedMult, 0f));
        }
    }
}