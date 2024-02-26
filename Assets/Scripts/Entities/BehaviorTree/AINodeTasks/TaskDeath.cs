using BehaviorTree;

namespace DTIS
{
    public class TaskDeath : Node
    {
        private readonly EntityController _AIcontroller;

        public TaskDeath(EntityController controller)
        {
            _AIcontroller = controller;
        }

        //in this eval function, we consider that the player is already in sight of the enemy. so there's no need to look for colliders.
        public override NodeState Evaluate()
        {

            if (_AIcontroller.HpBar.currentHp() <= 0) //enemy is dead
            {
                _AIcontroller.Animator.SetTrigger("Death");
                _AIcontroller.DropItems();
            }
            _state = NodeState.FAILURE;
            return _state;
        }
    }
}