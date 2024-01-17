using UnityEngine;

namespace DTIS
{
    public class IdleState:EntityState {
        public IdleState(string name = "Idle") 
        : base(name) 
        {

        }
        public override void Enter(EntityController controller)
        {
            // pass
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
            Debug.Log(Name);
            controller.Animator.Play(Name);
        }
    }
}