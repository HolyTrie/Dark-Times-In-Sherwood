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
    [SerializeField] private float _minGroundNormalY = 0.65f;

    protected bool grounded;
    protected Rigidbody2D _rb2d;
    protected Vector2 _velocity;
    protected Vector2 _targetVelocity;
    protected Vector2 _groundNormal;
    protected ContactFilter2D _contactFilter2d;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16]; // todo - variable length?
    protected List<RaycastHit2D> _hitBufferList = new(16);
    
    protected void OnEnable() {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _contactFilter2d.useTriggers = false;
        _contactFilter2d.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // get the Physics@d collision mask for this layer to filter by.
        _contactFilter2d.useLayerMask = true;
    }
    void Update()
    {
        _targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        _velocity += Time.deltaTime * _gravityModifier * Physics2D.gravity; // apply gravity
        _velocity.x = _targetVelocity.x;
        grounded = false;
        Vector2 deltaPostion = _velocity * Time.deltaTime;

        Vector2 moveAlongGround = new(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPostion;
        Movement(move, false); // horizontal movement
        
        move = Vector2.up * deltaPostion.y;

        Movement(move, true); // vertical movement
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rb2d.Cast(move, new ContactFilter2D(), _hitBuffer, distance + _shellRadius); // stores results into _hitBuffer and returns its length (can be discarded).
            _hitBufferList.Clear();
            for(int i = 0; i < count; ++i) // DO NOT Refactor this with foreach! it will iterate over empty spaces.
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }
            foreach(var hit in _hitBufferList)
            {
                Vector2 currentNormal = hit.normal;
                if(currentNormal.y > _minGroundNormalY) // if the normal vectors angle is greater then the set value.
                {
                    grounded = true;
                    if(yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal); // differnece between velocity and currentNormal to know how much to subtract if the player collides with a wall/ceiling
                if(projection < 0)
                {
                    _velocity -= projection * currentNormal;
                }

                float modifiedDistance = hit.distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }
        _rb2d.position += move.normalized * distance;
    }
}
