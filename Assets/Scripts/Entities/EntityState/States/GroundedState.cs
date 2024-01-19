using UnityEngine;

namespace DTIS
{
    public class GroundedState : EntityState
    {
        public GroundedState(string name = "Grounded") 
        : base(name){}
        public override void Exit(EntityController controller)
        {
            // pass
        }
        protected override void TryStateSwitch(EntityStateMachine fsm)
        {

        }
        protected override void PhysicsCalculation(EntityController controller,float Direction)
        {

        }
    }
}