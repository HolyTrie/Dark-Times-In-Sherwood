using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

public class GuardAI : BTree
{
    [Tooltip("Points that the guard will walk to")]
    [SerializeField] private Transform[] patrolTransforms;

    private EntityController _controller;
    protected override void Awake()
    {
        _controller = GetComponent<GuardController>(); // does NOT instantiate a Guard Controller if none exists!s
        patrolTransforms ??= new Transform[0];
        base.Awake(); // calls SetupTree
    }
    protected override Node SetupTree()
    {
        Node root = new BTreeBuilder()
            .Composite(new Selector())
                .Leaf(new TaskPatrol(patrolTransforms,_controller))
                .End
            .End
        .End;
        return root;
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