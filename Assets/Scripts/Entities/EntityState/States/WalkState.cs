using UnityEngine;

namespace DTIS
{
    public class WalkState:EntityState {
        public WalkState(string name = "Walk") 
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
            //controller.Animator.Play(Name);
            // if 
        }
        public override void FixedUpdate(EntityStateMachine fsm, EntityController controller)
        {
            Debug.Log(Name);
            controller.Animator.Play(Name);
        }
    }
}