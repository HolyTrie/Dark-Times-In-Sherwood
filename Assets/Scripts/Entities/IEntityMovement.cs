public interface IEntityMovement
{
    /*
        Use this interface to inject special mechanics into an entities movement, such as limping while moving, or no jumping, etc...
    */
    private float speedF, jumpF; //to be injected later.
    public void Move(float speedF);
    public void Jump(float JumpF);

}