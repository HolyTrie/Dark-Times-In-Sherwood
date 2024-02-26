using Unity.VisualScripting;

namespace DTIS
{
    public class FlyState : WalkState {
        public FlyState(ESP.States state,string name = "Fly") 
        : base(state,name) 
        {
            
        }
    }
}