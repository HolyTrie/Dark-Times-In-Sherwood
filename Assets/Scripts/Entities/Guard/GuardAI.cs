using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

public class GuardAI : BTree
{
    // Start is called before the first frame update
    private EntityController _controller;
    [SerializeField] private Transform[] patrolTranforms;

    protected override void Awake(){
        _controller = GetComponent<GuardController>(); // does NOT instantiate a Guard Controller if none exists!s
        base.Awake(); // calls SetupTree
    }
    protected override Node SetupTree()
    {
        Node root = new BTreeBuilder()
            .Composite(new Selector())
                .Leaf(new TaskPatrol(patrolTranforms,_controller))
                .End
            .End
        .End;
        return root;
    }
}

internal class TaskPatrol : Node
{
    private readonly EntityController _controller;
    private readonly Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    public TaskPatrol(Transform[] waypoints, EntityController controller)
    {
        _controller = controller;
        _waypoints = waypoints;
    }

    public override NodeState Evaluate()
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
                _controller.transform.position = new Vector2(wp.position.x,_controller.transform.position.y);
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _controller.Animator.SetInteger("AnimState", 0);
            }
            else
            {
                //_controller.transform.position = Vector2.MoveTowards(_controller.transform.position, new Vector2(wp.position.x,_controller.transform.position.y), _controller.WalkSpeed * Time.deltaTime);
                // _controller.transform.LookAt(new Vector3(wp.position.x,_controller.transform.position.y,_controller.transform.position.z));
                var direction = _controller.transform.position.x < wp.position.x ? 1.0f: -1.0f;
                _controller.Move(new Vector2(direction,0.25f));
            }
        }


        _state = NodeState.RUNNING;
        return _state;
    }
}