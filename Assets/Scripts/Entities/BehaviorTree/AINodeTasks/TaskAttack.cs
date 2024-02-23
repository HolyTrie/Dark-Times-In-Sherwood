using BehaviorTree;
using UnityEngine;

namespace DTIS
{
    public class TaskAttack : Node
    {

        private readonly EntityController _AIcontroller;
        private Transform _lastTarget;
        private PlayerController player;


        //timers for delays between attacks//
        private float _attackTime = 1f; // how long it takes to attack with animation
        private float _attackCounter = 0f;

        public TaskAttack(EntityController controller)
        {
            _AIcontroller = controller;
        }

        //in this eval function, we consider that the player is already in sight of the enemy. so there's no need to look for colliders.
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            Debug.Log(target.parent.name);
            if (target != _lastTarget)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                _lastTarget = target;
            }

            _attackCounter += Time.deltaTime;
            if (_attackCounter >= _attackTime)
            {
                player.HpBar.depleteHp(_AIcontroller.AttackDMG); // should make this better in terms of hit with collider maybe?
                if (player.HpBar.currentHp() <= 0) // player is dead
                {
                    ClearData("target");
                    _AIcontroller.Animator.SetInteger("AnimState", 0);
                }
                else
                    _attackCounter = 0f;
            }

            _state = NodeState.RUNNING;
            return _state;
        }

    }
}