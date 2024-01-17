namespace DTIS
{
    /// <summary>
    /// Every state has 3 key responsibilities which are unique to it's implementation.
    /// 1. Setting the correct graphic/animation in the Animator component via a property of the controller.
    /// 2. Checking conditions to switch to legal states and sub-states, and making the switch via properties in the FSM.
    /// 3. update the entity behaviour via the controller in every update and fixed update, for example walk and run manipulate the velocity of an entity.
    /// </summary>
    public abstract class EntityState
    {
        public EntityState()
        {

        }
        public abstract void Enter(EntityController controller);
        public abstract void Exit(EntityController controller);
        public abstract void Update(EntityStateMachine fsm, EntityController controller);
        public abstract void FixedUpdate(EntityStateMachine fsm, EntityController controller);
    }
}
