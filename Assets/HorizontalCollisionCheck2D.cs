using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalCollisionCheck2D : MonoBehaviour
{
    public enum CollisionType 
    {
        NONE,
        FULL,
        PARTIAL
    }

    [Header("General Setting (Applies to both sides)")]

    private const int _maxRays = 16;
    [Tooltip("Important - Only layers enabled by the layer mask will be checked against.")]
    [SerializeField] protected private LayerMask _layerMask;
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    
    [Header("Left side raycasts settings")]

    protected private CollisionType _leftCollisionType = CollisionType.NONE;
    public CollisionType LeftCollisionType{get{return _leftCollisionType;}}
    protected List<RaycastHit2D> _leftRayHitBufferList = new(_maxRays);
    [SerializeField] private Transform _leftParentTop;
    [SerializeField] private Transform _leftParentBottom;
    private readonly Vector2[] _leftRayOffsets = new Vector2[_maxRays];
    [SerializeField, Range(0,_maxRays)] private int _leftRayCount = 5;
    [SerializeField] private float  _leftRaysOffsetX;
    [SerializeField] private float _leftRaysCastDistance;
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    
    [Header("Right side raycasts settings")]

    protected private CollisionType _rightCollisionType = CollisionType.NONE;
    public CollisionType RightCollisionType{get{return _rightCollisionType;}}
    protected private List<RaycastHit2D> _rightRayHitBufferList = new(_maxRays);
    [SerializeField] private Transform _rightParentTop;
    [SerializeField] private Transform _rightParentBottom;
    private readonly Vector2[] _rightRayOffsets = new Vector2[_maxRays];
    [SerializeField, Range(0,_maxRays)] private int _rightRayCount = 5;
    [SerializeField] private float  _rightRaysOffsetX;
    [SerializeField] private float _rightRaysCastDistance;
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    private void Awake() 
    {
        InitRightRays();
        InitLeftRays();
    }
    private void InitRightRays()
    {
        if (_rightRayCount == 0) 
            return;
        float height = Mathf.Abs(Vector2.Distance(_rightParentBottom.position,_rightParentTop.position));
        float raySpacing = height / _rightRayCount;
        for(int i = 0 ; i<_rightRayCount ; ++i)
        {
            var _x = _rightRaysOffsetX;
            float _y = (i+1) * raySpacing;
            _rightRayOffsets[i] = new Vector2(_x,_y); //replace the previous vector!
        }
    }
    private void InitLeftRays()
    {
        if (_leftRayCount == 0) 
            return;
        float height = Mathf.Abs(Vector2.Distance(_leftParentBottom.position,_leftParentTop.position));
        float raySpacing = height / _leftRayCount;
        for(int i = 0 ; i<_leftRayCount ; ++i)
        {
            var _x = _leftRaysOffsetX;
            var _y = (i+1) * raySpacing;
            _leftRayOffsets[i] = new Vector2(_x,_y); //replace the previous vector!
        }
    }
    void FixedUpdate()
    {
        CastLeftRays();
        CastRightRays();
    }
    private void CastLeftRays()
    {
        if (_leftRayCount == 0) 
            return;
        _leftRayHitBufferList.Clear();
        var origin = _leftParentBottom.position;
        Vector2 pos;
        for(int i =0; i<_leftRayCount; ++i)
        {
            pos = new(origin.x+_leftRayOffsets[i].x,origin.y+_leftRayOffsets[i].y);
            _leftRayHitBufferList.Add(Physics2D.Raycast(pos,Vector2.left,_leftRaysCastDistance,_layerMask));
            //Debug.DrawRay(pos,Vector2.left,Color.blue);
        }
        int count = 0;
        foreach (var hit in _leftRayHitBufferList)
        {
            if(!hit)
                continue;
            ++count;
        }
        if(count == 0)
        {
            _leftCollisionType = CollisionType.NONE;
            return;
        }
        if(count == _leftRayCount)
        {
            _leftCollisionType = CollisionType.FULL;
            return;
        }
        _leftCollisionType = CollisionType.PARTIAL;
    }
    private void CastRightRays()
    {
        if (_rightRayCount == 0) 
            return;
        _rightRayHitBufferList.Clear();
        var origin = _rightParentBottom.position;
        Vector2 pos;
        for(int i =0; i<_rightRayCount; ++i)
        {
            pos = new(origin.x+_rightRayOffsets[i].x,origin.y+_rightRayOffsets[i].y);
            _rightRayHitBufferList.Add(Physics2D.Raycast(pos,Vector2.right,_rightRaysCastDistance,_layerMask));
            //Debug.DrawRay(pos,Vector2.right,Color.red);
        }
        int count = 0;
        foreach (var hit in _rightRayHitBufferList)
        {
            if(!hit)
                continue;
            ++count;
        }
        if(count == 0)
        {
            _rightCollisionType = CollisionType.NONE;
            return;
        }
        if(count == _rightRayCount)
        {
            _rightCollisionType = CollisionType.FULL;
            return;
        }
        _rightCollisionType = CollisionType.PARTIAL;
    }
}
