namespace DTIS
{
    public class JumpState : EntityState
    {
        public JumpState(string name = "Jump") 
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
            //controller.Move(new Vector2(0f,Direction * controller.RunSpeedMult)); // problematic
        }
    }
}