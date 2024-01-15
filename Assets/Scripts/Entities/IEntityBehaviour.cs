public interface IEntityBehaviour
{
    public IEntityMovement moveStrategy = DemoMovement; 

    public void onPlayerSighted();
    public void onLostSightOfPlayer();
    public void Move();
    public void Attack();
}