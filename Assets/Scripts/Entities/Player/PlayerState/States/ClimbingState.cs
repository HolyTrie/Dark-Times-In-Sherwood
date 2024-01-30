using UnityEngine;

namespace DTIS
{
    public class ClimbingState : PlayerState
    {
        public ClimbingState(string name = "Climbing") 
        : base(name,false){}
        public override void Enter(PlayerController controller,PlayerStateMachine fsm)
        {
            base.Enter(controller,fsm); // we do new to preserve the common inherited function
        }
        protected override void TryStateSwitch()
        {
        }
        protected override void PhysicsCalculation()
        {
            //pass
        }
    }
}