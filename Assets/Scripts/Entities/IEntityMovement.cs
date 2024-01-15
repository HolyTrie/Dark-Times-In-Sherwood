namespace DTIS
{
    public interface IEntityMovement
    {
        public void Walk(EntityController con);
        public void Jump(EntityController con);
        public void Run(EntityController con);

    }
}
