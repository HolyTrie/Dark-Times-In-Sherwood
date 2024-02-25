using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

namespace DTIS
{
    public class CheckPlayerInAttackRange : Node
    {
        private readonly EntityController _AIcontroller;

        public CheckPlayerInAttackRange(EntityController controller)
        {
            _AIcontroller = controller;
        }

        //in this eval function, we consider that the player is already in sight of the enemy. so there's no need to look for colliders.
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target == null || Math.Abs(_AIcontroller.transform.position.x - target.position.x) > _AIcontroller.AttackRange) // player not in range or target is not available.
            {
                // Debug.Log("Player not in attack range");
                _state = NodeState.FAILURE;
                return _state;
            }

            if (Math.Abs(_AIcontroller.transform.position.x - target.position.x) <= _AIcontroller.AttackRange) // this indicates the player is in range of the enemy, so it can attack him
            {
                _AIcontroller.Animator.SetInteger("AnimState", 3);

                _state = NodeState.SUCCESS;
                return _state;
            }
            _state = NodeState.FAILURE;
            return _state;
        }
    }
}