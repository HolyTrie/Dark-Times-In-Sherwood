public interface IEntityStateMachine
{
    private enum states;
    private int state;

    public int getState();
    public void setState(int state);
}