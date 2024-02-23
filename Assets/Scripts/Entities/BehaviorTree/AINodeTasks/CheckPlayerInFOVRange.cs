using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

namespace DTIS
{
    public class CheckPlayerInFOVRange : Node
    {
        private readonly EntityController _AIcontroller;
        private static int _PlayerLayerMask = 1 << 3; // tbd 

        public CheckPlayerInFOVRange(EntityController controller)
        {
            _AIcontroller = controller;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_AIcontroller.transform.position, _AIcontroller.FieldOfView, _PlayerLayerMask);

                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("target", colliders[0].transform);
                    _AIcontroller.Animator.SetInteger("AnimState", 2);
                    _state = NodeState.SUCCESS; // we have a target, so its a success
                    return _state;
                }

                _state = NodeState.FAILURE; // we dont have a target, so state "fails"
                return _state;
            }

            _state = NodeState.SUCCESS; // we have a target, so its a success
            return _state;
        }
    }
}