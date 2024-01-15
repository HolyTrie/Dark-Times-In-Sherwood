namespace DTIS
{
    public interface IEntityBehaviour
    {
        //public void onPlayerSighted();
        //public void onLostSightOfPlayer();
        //public void Attack();
        public void Idle();
        public void Update(IEntityMovement movement, EntityStateMachine fsm);
        public void FixedUpdate(IEntityMovement movement, EntityStateMachine fsm, EntityController con);
    }
}