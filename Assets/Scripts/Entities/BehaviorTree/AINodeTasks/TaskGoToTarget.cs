using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

namespace DTIS
{
    public class TaskGoToTarget : Node
    {
        private readonly EntityController _AIcontroller;

        public TaskGoToTarget(EntityController controller)
        {
            _AIcontroller = controller;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");


            if (target == null || Math.Abs(_AIcontroller.transform.position.x - target.position.x) > _AIcontroller.FieldOfView) // this indicates the player ran from the enemy so he stops chasing
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            if (Math.Abs(_AIcontroller.transform.position.x - target.position.x) > 0.01f) //player is nearby enemy, so it will chase him
            {
                float direction = _AIcontroller.transform.position.x < target.position.x ? 1.0f : -1.0f;
                _AIcontroller.Move(new Vector2(direction, 0f));

                _AIcontroller.Flip(target.position.x); // flips the entity according the the position x of target.

                _AIcontroller.Animator.SetInteger("AnimState", 2);
                _state = NodeState.RUNNING;
                return _state;
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}