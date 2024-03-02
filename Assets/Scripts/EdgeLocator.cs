using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeLocator : MonoBehaviour
{
    public bool Hit { get { return _hit; } }
    [Header("Layer Mask")]
    [SerializeField]
    protected private LayerMask _whatIsGround;
    [Header("Ray Settings")]
    [SerializeField, Tooltip("will be flipped symmetrically for left position reference as well!")]
    private Transform _rightRayOrigin;
    [SerializeField, Tooltip("will be flipped symmetrically for left position reference as well!")]
    private Transform _rightRayDestination;
    protected private Vector2 _rayOrigin;
    protected private Vector2 _rayDestination;
    protected private Vector2 _rayCastDirection;
    protected private float _rayCastDistance;
    protected private bool _hit;
    protected virtual void Start()
    {
        _rayOrigin = new(_rightRayOrigin.position.x, _rightRayOrigin.position.y);
        _rayDestination = new(_rightRayDestination.position.x, _rightRayDestination.position.y);
        _rayCastDistance = Vector2.Distance(_rayOrigin, _rayDestination);
        _rayCastDirection = _rayOrigin.y > _rayDestination.y ? -Vector2.up : Vector2.up;
    }
    protected virtual void FixedUpdate()
    {
        _rayOrigin.x = _rightRayOrigin.position.x;
        _rayOrigin.y = _rightRayOrigin.position.y;
        _rayDestination.x = _rightRayDestination.position.x;
        _rayDestination.y = _rightRayDestination.position.y;
        var hit = Physics2D.Raycast(_rayOrigin, _rayCastDirection, _rayCastDistance, _whatIsGround);
        _hit = hit;
        //Debug.Log("Edge locator hit =" + (bool)hit);
        //Debug.DrawLine(_rayOrigin, _rayDestination, Color.cyan);
    }
}
