using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEditor.PackageManager;
using UnityEngine;

public class GuardAI : BTree
{
    [Tooltip("Points that the guard will walk to")]
    [SerializeField] public Transform[] patrolTransforms;

    [SerializeField] public static float fovRange = 6f;

    private EntityController _controller;
    protected override void Awake()
    {
        _controller = GetComponent<GuardController>(); // does NOT instantiate a Guard Controller if none exists!s
        patrolTransforms ??= new Transform[0];
        base.Awake(); // calls SetupTree
    }
    protected override Node SetupTree()
    {
        // Node root = new BTreeBuilder()
        //     .Composite(new Selector())
        //         .Leaf(new TaskPatrol(patrolTransforms, _controller))
        //         .End
        //     .End
        // .End;

        Node root = new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInFOVRange(transform,_controller),
                        new TaskGoToTarget(transform,_controller),
                    }),
                    new TaskPatrol(patrolTransforms,_controller),
                });
        return root;
    }
}

internal class TaskGoToTarget : Node
{
    private readonly EntityController _controller;
    private Transform _transform;
    private float patrolRange = 10f;

    public TaskGoToTarget(Transform transform, EntityController controller)
    {
        _transform = transform;
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        

        if (target == null || Vector3.Distance(_controller.transform.position, target.position) > patrolRange) // this indicates the player ran from the enemy so he stops chasing
        {
            _state = NodeState.FAILURE;
            return _state;
        }

        if (Math.Abs(_controller.transform.position.x - target.position.x) > 0.01f) //player is nearby enemy, so it will chase him
        {
            float direction = _controller.transform.position.x < target.position.x ? 1.0f : -1.0f;
            _controller.Move(new Vector2(direction, 0f));

            _state = NodeState.RUNNING;
            return _state;
        }

        _state = NodeState.FAILURE;
        return _state;
    }

}


internal class CheckPlayerInFOVRange : Node
{
    private readonly EntityController _controller;
    private static int _PlayerLayerMask = 1 << 3; // tbd 
    private Transform _transform;

    public CheckPlayerInFOVRange(Transform transform, EntityController controller)
    {
        _transform = transform;
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, GuardAI.fovRange, _PlayerLayerMask);

            if (colliders.Length > 0)
            {
                Parent.Parent.SetData("target", colliders[0].transform);
                _controller.Animator.SetInteger("AnimState", 2);
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


internal class TaskPatrol : Node
{
    private readonly EntityController _controller;
    private float _prevX;
    private int stuckCounter = 0;
    private readonly Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    public TaskPatrol(Transform[] waypoints, EntityController controller)
    {
        _controller = controller;
        _prevX = _controller.transform.position.x;
        _waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (_waypoints != null)
        {
            if (_waypoints.Length != 0)
            {
                if (_waiting)
                {
                    _waitCounter += Time.deltaTime;
                    if (_waitCounter >= _waitTime)
                    {
                        _waiting = false;
                        _controller.Animator.SetInteger("AnimState", 2);
                    }
                }
                else
                {
                    Transform wp = _waypoints[_currentWaypointIndex];
                    if (wp != null)
                    {
                        if (Math.Abs(_controller.transform.position.x - wp.position.x) < 0.01f)
                        {
                            _controller.transform.position = new Vector2(wp.position.x, _controller.transform.position.y);
                            _waitCounter = 0f;
                            _waiting = true;

                            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                            _controller.Animator.SetInteger("AnimState", 0);
                        }
                        else
                        {
                            float direction = _controller.transform.position.x < wp.position.x ? 1.0f : -1.0f;
                            _controller.Move(new Vector2(direction, 0f));

                            if (Math.Abs(_controller.transform.position.x - _prevX) < 0.01f)
                            {
                                stuckCounter += 1;
                                if (stuckCounter > 2)
                                {
                                    Nudge(new Vector2(direction, 0.05f));
                                    stuckCounter = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        _prevX = _controller.transform.position.x;
        _state = NodeState.RUNNING;
        return _state;
    }

    private void Nudge(Vector2 direction)
    {
        Vector2 newPos = _controller.transform.position;
        newPos.x += direction.x * Time.deltaTime;
        newPos.y += direction.y * Time.deltaTime;
        _controller.transform.position = newPos;
    }
}