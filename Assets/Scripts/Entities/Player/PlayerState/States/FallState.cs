namespace DTIS
{
    public class FallState : PlayerState {
        public FallState(string name = "Fall") 
        : base(name,false){}
        protected override void TryStateSwitch()
        {
            // if(Controller.Velocity.y == 0)
            // {
            //     SetStates(ESP.States.Grounded,ESP.States.Idle);
            // }
        }
        protected override void PhysicsCalculation()
        {
            
        }
    }
}