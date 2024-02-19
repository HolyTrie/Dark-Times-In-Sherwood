using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour
{
    // source - https://www.youtube.com/watch?v=wGI2e3Dzk_w&list=PLX2vGYjWbI0SUWwVPCERK88Qw8hpjEGd8&index=1&ab_channel=Unity
    protected const float _minMoveDistance = 0.001f;
    protected const float _shellRadius = 0.01f;
    [SerializeField] private float _gravityModifier = 1f;
    protected Rigidbody2D _rb2d;
    protected Vector2 _velocity;
    protected ContactFilter2D _contactFilter2d;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16]; // todo - variable length?
    protected List<RaycastHit2D> _hitBufferList = new(16);
    protected void OnEnable() {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    protected void Start()
    {
        _contactFilter2d.useTriggers = false;
        _contactFilter2d.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // get the attached game objects collision mask to filter by.
        _contactFilter2d.useLayerMask = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        //TODO: compare with Time.fixedDetlaTime !
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPostion = _velocity * Time.deltaTime;
        Vector2 move = Vector2.up * deltaPostion.y;
        Movement(move);
    }

    void Movement(Vector2 move)
    {
        float distance = move.magnitude;
        if (distance > _minMoveDistance)
        {
            int rayCount = _rb2d.Cast(move, _contactFilter2d, _hitBuffer, distance + _shellRadius);


        }
        _rb2d.position = _rb2d.position + move;
    }
}
