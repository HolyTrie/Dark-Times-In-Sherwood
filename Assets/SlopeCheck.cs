using System;
using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    public bool SlopeAhead { get { return _slopeAhead; } }
    public bool IsSlopeUpwardsLeftToRight { get { return _isSlopeUpwardsLeftToRight; } }

    [Header("Ray Settings")]
    [Tooltip("will be flipped symmetrically for left position reference as well!")]
    [SerializeField] private Transform _rightRayPosRef;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _rayCastDistance = 1f;
    private Vector2 _rayPos;
    //private bool _isRight = false;
    private bool _slopeAhead = false;
    private bool _isSlopeUpwardsLeftToRight = false;
    //private PlayerController _pc;
    void Start()
    {
        SetRayParams();
    }
    void FixedUpdate()
    {
        SetRayParams();
    }
    private void SetRayParams()
    {
        _rayPos = new(_rightRayPosRef.position.x,_rightRayPosRef.position.y);
        var hit = Physics2D.Raycast(_rayPos,-Vector2.up,_rayCastDistance,_layerMask);
        Debug.DrawRay(_rayPos,-Vector2.up * _rayCastDistance, Color.cyan);
        if(hit)
        {
            var normalAngle = Vector2.Angle(hit.point,Vector2.up);
            var absAngle = Mathf.Abs(normalAngle);
            if(absAngle > 0 && absAngle < 1)
            {
                _slopeAhead = true;
            }
            if(_slopeAhead)
            {
                if(normalAngle > 0)
                    _isSlopeUpwardsLeftToRight = false;
                else
                    _isSlopeUpwardsLeftToRight = true;
            }
        }
        else
            _slopeAhead = false;
    }
}
