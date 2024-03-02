using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

namespace DTIS
{
    public class TaskFlyToTarget : Node
    {
        private readonly EntityController _AIcontroller;
        private Vector3 _prevPos;
        private int stuckCounter = 0;

        public TaskFlyToTarget(EntityController controller)
        {
            _AIcontroller = controller;
            _prevPos = _AIcontroller.transform.position;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");


            if (target == null || Vector3.Distance(_AIcontroller.transform.position,target.position) > _AIcontroller.FieldOfView) // this indicates the player ran from the enemy so he stops chasing
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            if (Vector3.Distance(_AIcontroller.transform.position,target.position) > 0.01f) //player is nearby enemy, so it will chase him
            {
                Vector3 directionVector = (target.position - _AIcontroller.transform.position).normalized;
                Debug.Log("DIRECTION VECTOR:" + directionVector);
                _AIcontroller.Move(directionVector);

                _AIcontroller.Flip(target.position.x); // flips the entity according the the position x of target.

                _AIcontroller.Animator.SetInteger("AnimState", 2);

                if (Vector3.Distance(_AIcontroller.transform.position,_prevPos) < 0.01f)
                {
                    stuckCounter += 1;
                    if (stuckCounter > 2)
                    {
                        Nudge(new Vector2(directionVector.x, 0.05f));
                        stuckCounter = 0;
                    }
                }
                _prevPos = _AIcontroller.transform.position;
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