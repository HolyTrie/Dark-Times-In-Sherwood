using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowTransform : MonoBehaviour
{
    [SerializeField] Transform _transformToFollow;
    private const float _snapThresholdMult = 1.5f; 
    private Vector3 _fixedPos;
    private float _initialDistance;
    Rigidbody2D _rb2d;
    void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _fixedPos = transform.localPosition;
        _initialDistance = Vector2.Distance(transform.position,_transformToFollow.position);
    }
    void FixedUpdate()
    {
        if(_initialDistance * _snapThresholdMult <= Vector2.Distance(transform.position,_transformToFollow.position))
            Util.MimicEntityMovement(transform,_transformToFollow,_fixedPos);
        _rb2d.MovePosition(_transformToFollow.position);
    }
}
