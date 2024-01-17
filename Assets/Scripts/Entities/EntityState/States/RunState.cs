namespace DTIS
{
    public class RunState:EntityState {
        public RunState(string name = "Run") 
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
            // pass
        }
    }
}