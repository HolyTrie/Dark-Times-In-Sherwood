namespace DTIS
{
    public class FlyState : WalkState {
        public FlyState(string name = "Fly") 
        : base(name) 
        {
            
        }
        public override void Enter(EntityController controller)
        {
            //controller.setGraphic("flying");
        }
    }
}