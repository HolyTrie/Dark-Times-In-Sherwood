using UnityEngine;

namespace DTIS
{
    public class GroundedState : EntityState
    {
        public GroundedState(string name = "Grounded") 
        : base(name) 
        {

        }
        public override void Enter(EntityController controller)
        {
            Debug.Log("Enter Grounded State");
        }
        public override void Exit(EntityController controller)
        {
            // pass
        }
        public override void Update(EntityStateMachine fsm, EntityController controller)
        {
            // pass
        }
        public override void FixedUpdate(EntityStateMachine fsm, EntityController controller)
        {
            // pass
        }
    }
}