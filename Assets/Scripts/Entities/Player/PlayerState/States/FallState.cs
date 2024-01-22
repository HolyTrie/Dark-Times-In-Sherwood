namespace DTIS
{
    public class FallState : PlayerState {
        public FallState(string name = "Fall") 
        : base(name){}
        public override void Exit()
        {
            // pass
        }
        protected override void TryStateSwitch()
        {
            /*
            if(Controller.Velocity.y >= 0)
            {
                SetStates(ESP.States.Grounded,ESP.States.Idle);
            }
            */
        }
        protected override void PhysicsCalculation()
        {
            //controller.Move(new Vector2(0f,Direction * controller.RunSpeedMult)); // problematic
        }
    }
}