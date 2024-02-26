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
        private float _prevX;
        private int stuckCounter = 0;

        public TaskGoToTarget(EntityController controller)
        {
            _AIcontroller = controller;
            _prevX = _AIcontroller.transform.position.x;
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

                if (Math.Abs(_AIcontroller.transform.position.x - _prevX) < 0.01f)
                {
                    stuckCounter += 1;
                    if (stuckCounter > 2)
                    {
                        Nudge(new Vector2(direction, 0.05f));
                        stuckCounter = 0;
                    }
                }
                _prevX = _AIcontroller.transform.position.x;
                _state = NodeState.RUNNING;
                return _state;
            }

            _state = NodeState.FAILURE;
            return _state;
        }

        private void Nudge(Vector2 direction)
        {
            Vector2 newPos = _AIcontroller.transform.position;
            newPos.x += direction.x * Time.deltaTime;
            newPos.y += direction.y * Time.deltaTime;
            _AIcontroller.transform.position = newPos;
        }
    }
}