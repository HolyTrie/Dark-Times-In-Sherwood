using UnityEngine.InputSystem.Utilities;

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
        private string _name;
        public EntityState(string name)
        {
            _name = name;
        }
        public virtual void Enter(EntityController controller)
        {
            try
            {
                controller.Animator.Play(Name);
            }
            catch
            {}
        }
        public abstract void Exit(EntityController controller);
        public virtual void Update(EntityStateMachine fsm)
        {
            TryStateSwitch(fsm);
        }
        public virtual void FixedUpdate(EntityController controller, float Direction)
        {
            PhysicsCalculation(controller,Direction);
        }
        protected abstract void TryStateSwitch(EntityStateMachine fsm /*Todo: how to best pass input*/); // this should check input from EntityBrain and switch states appropriately
        protected abstract void PhysicsCalculation(EntityController controller,float Direction); // This is a set method to update

        public virtual string Name {get { return _name; }}
    }
}
