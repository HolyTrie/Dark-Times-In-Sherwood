using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : PhysicsObject
{
    [SerializeField] private float _maxSpeed = 7;
    [SerializeField] private float _jumpTakeOffSpeed = 7f;
    private Vector2 _move = Vector2.zero;
    public Vector2 Move {get{return _move;}set{_move = value;}}
    public Vector2 Velocity {get{return _velocity;}set{_velocity = value;}}
    protected override void ComputeVelocity()
    {
        _move = Vector2.zero;

        _move.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && grounded)
        {
            _velocity.y = _jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if(_velocity.y > 0)
            {
                _velocity.y *= 0.5f;
            }
        }
        _targetVelocity = _move * _maxSpeed;
    }

    internal void Jump()
    {
        _velocity.y = _jumpTakeOffSpeed;
    }

    internal void DeaccelarateJump()
    {
        _velocity.y *= 0.5f;
    }
}
