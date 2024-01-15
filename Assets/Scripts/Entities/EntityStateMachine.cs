namespace DTIS
{
    public abstract class EntityStateMachine
    {
        // TODO: implement this more appropriatly!
        public enum states{};
        private int curr_state;

        public int getState()
        {
            return curr_state;
        }
        public void setState(int state)
        {
            curr_state = state;
        }
    }
}