using System;
using System.Collections.Generic;
using BehaviorTree;
using DTIS;
using UnityEngine;

public class AIPatrolChaseAttack : BTree
{
    [Tooltip("Points that the guard will walk to")]
    [SerializeField] public Transform[] patrolTransforms;

    private EntityController _AIcontroller;
    protected override void Awake()
    {
        _AIcontroller = GetComponent<EntityController>(); // does NOT instantiate a Guard Controller if none exists!s
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
                        new CheckPlayerInAttackRange(_AIcontroller),
                        new TaskAttack(_AIcontroller),
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInFOVRange(_AIcontroller),
                        new TaskGoToTarget(_AIcontroller),
                    }),
                    new TaskPatrol(patrolTransforms,_AIcontroller),
                });
        return root;
    }
}