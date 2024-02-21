using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

public class GuardAI : BTree
{
    [Tooltip("Points that the guard will walk to")]
    [SerializeField] public Transform[] patrolTransforms;

    [Tooltip("The range that the entity can see and start chasing the player")]
    [SerializeField] public static float fovRange = 6f;

    [Tooltip("The range that the entity will start attacking")]
    [SerializeField] public static float attackRange = 1f;

    private EntityController _controller;
    protected override void Awake()
    {
        _controller = GetComponent<EntityController>(); // does NOT instantiate a Guard Controller if none exists!s
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
                        new CheckPlayerInAttackRange(_controller),
                        new TaskAttack(_controller),
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInFOVRange(_controller),
                        new TaskGoToTarget(_controller),
                    }),
                    new TaskPatrol(patrolTransforms,_controller),
                });
        return root;
    }
}

internal class TaskAttack : Node
{

    private readonly EntityController _controller;
    private Transform _lastTarget;
    private PlayerController player;


    //timers for delays between attacks//
    private float _attackTime = 1f; // how long it takes to attack with animation
    private float _attackCounter = 0f;

    public TaskAttack(EntityController controller)
    {
        _controller = controller;
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
            player.HpBar.depleteHp(_controller.AttackDMG); // should make this better in terms of hit with collider maybe?
            if (player.HpBar.currentHp() <= 0) // player is dead
            {
                ClearData("target");
                _controller.Animator.SetInteger("AnimState", 0);
            }
            else
                _attackCounter = 0f;
        }

        _state = NodeState.RUNNING;
        return _state;
    }

}

internal class CheckPlayerInAttackRange : Node
{
    private readonly EntityController _controller;

    public CheckPlayerInAttackRange(EntityController controller)
    {
        _controller = controller;
    }

    //in this eval function, we consider that the player is already in sight of the enemy. so there's no need to look for colliders.
    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target == null || Math.Abs(_controller.transform.position.x - target.position.x) > GuardAI.attackRange) // player not in range or target is not available.
        {
            Debug.Log("Player not in attack range");
            _state = NodeState.FAILURE;
            return _state;
        }

        if (Math.Abs(_controller.transform.position.x - target.position.x) <= GuardAI.attackRange) // this indicates the player is in range of the enemy, so it can attack him
        {
            Debug.Log("Player in attack range" + Math.Abs(_controller.transform.position.x - target.position.x));
            _controller.Animator.SetInteger("AnimState", 3);

            _state = NodeState.SUCCESS;
            return _state;
        }
        _state = NodeState.FAILURE;
        return _state;
    }
}

internal class TaskGoToTarget : Node
{
    private readonly EntityController _controller;

    public TaskGoToTarget(EntityController controller)
    {
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");


        if (target == null || Math.Abs(_controller.transform.position.x - target.position.x) > GuardAI.fovRange) // this indicates the player ran from the enemy so he stops chasing
        {
            _state = NodeState.FAILURE;
            return _state;
        }

        if (Math.Abs(_controller.transform.position.x - target.position.x) > 0.01f) //player is nearby enemy, so it will chase him
        {
            float direction = _controller.transform.position.x < target.position.x ? 1.0f : -1.0f;
            _controller.Move(new Vector2(direction, 0f));

            _controller.Flip(target.position.x); // flips the entity according the the position x of target.

            _controller.Animator.SetInteger("AnimState", 2);
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

    public CheckPlayerInFOVRange(EntityController controller)
    {
        _controller = controller;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_controller.transform.position, GuardAI.fovRange, _PlayerLayerMask);

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

                        _controller.Flip(wp.position.x); // flips the entity according the the position x of target.

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