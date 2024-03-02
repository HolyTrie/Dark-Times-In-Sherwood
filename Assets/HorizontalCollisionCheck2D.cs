using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalCollisionCheck2D : MonoBehaviour
{
    
    public enum CollisionTypes
    {
        NONE,
        FULL,
        PARTIAL
    }
    
    private void Awake()
    {
        InitRightRays();
        InitLeftRays();
    }
    void FixedUpdate()
    {
        CastLeftRays();
        CastRightRays();
    }

    [Header("General Setting (Applies to both sides)")]

    private const int _maxRays = 16;
    [Tooltip("Important - Only layers enabled by the layer mask will be checked against.")]
    [SerializeField] protected private LayerMask _layerMask;
    
    #region LEFT SIDE
    public bool TopLeftExtremeHit { get { return _topLeftExtremeHit; } }
    public bool BottomLeftExtremeHit { get { return _bottomLeftExtremeHit; } }
    public CollisionTypes LeftCollisionType { get { return _leftCollisionType; } }

    [Header("Left side raycasts settings")]

    [SerializeField] 
    private Transform _leftParentTop;
    [SerializeField] 
    private Transform _leftParentBottom;
    [SerializeField, Range(0, _maxRays)] 
    private int _leftRayCount = 5;
    [SerializeField] 
    private float _leftRaysOffsetX;
    [SerializeField] 
    private float _leftRaysCastDistance;
    private bool _topLeftExtremeHit = false;
    private bool _bottomLeftExtremeHit = false;
    protected private CollisionTypes _leftCollisionType = CollisionTypes.NONE;
    protected List<RaycastHit2D> _leftRayHitBufferList = new(_maxRays);
    private readonly Vector2[] _leftRayOffsets = new Vector2[_maxRays];
    private void InitLeftRays()
    {
        if (_leftRayCount == 0)
            return;
        float height = Mathf.Abs(Vector2.Distance(_leftParentBottom.position, _leftParentTop.position));
        float raySpacing = height / _leftRayCount;
        for (int i = 0; i < _leftRayCount; ++i)
        {
            var _x = _leftRaysOffsetX;
            var _y = (i + 1) * raySpacing;
            _leftRayOffsets[i] = new Vector2(_x, _y); //replace the previous vector!
        }
    }
    private void CastLeftRays()
    {
        if (_leftRayCount == 0)
            return;
        _leftRayHitBufferList.Clear();
        var origin = _leftParentBottom.position; //start the cast from the bottom uup!
        Vector2 pos;
        int count = 0;
        RaycastHit2D hit;
        for (int i = 0; i < _leftRayCount; ++i)
        {
            pos = new(origin.x + _leftRayOffsets[i].x, origin.y + _leftRayOffsets[i].y);
            hit = Physics2D.Raycast(pos, Vector2.left, _leftRaysCastDistance, _layerMask);
            if(hit)
            {
                ++count;
                _leftRayHitBufferList.Add(hit);
            }
            if(i == 0)
                _bottomLeftExtremeHit = hit;
            if(i == _leftRayCount - 1)
                _topLeftExtremeHit = hit;
        }
        if (count == 0)
        {
            _leftCollisionType = CollisionTypes.NONE;
            return;
        }
        if (count == _leftRayCount)
        {
            _leftCollisionType = CollisionTypes.FULL;
            return;
        }
        _leftCollisionType = CollisionTypes.PARTIAL;
    }

    #endregion

    #region RIGHT SIDE
    public bool TopRightExtremeHit { get { return _topLeftExtremeHit; } }
    public bool BottomRightExtremeHit { get { return _bottomLeftExtremeHit; } }
    public CollisionTypes RightCollisionType { get { return _rightCollisionType; } }

    [Header("Right side raycasts settings")]
    [SerializeField] 
    private Transform _rightParentTop;
    [SerializeField] 
    private Transform _rightParentBottom;
    [SerializeField, Range(0, _maxRays)] 
    private int _rightRayCount = 5;
    [SerializeField] 
    private float _rightRaysOffsetX;
    [SerializeField] 
    private float _rightRaysCastDistance;
    private readonly Vector2[] _rightRayOffsets = new Vector2[_maxRays];
    protected private CollisionTypes _rightCollisionType = CollisionTypes.NONE;
    protected private List<RaycastHit2D> _rightRayHitBufferList = new(_maxRays);
    private bool _topRightExtremeHit = false;
    private bool _bottomRightExtremeHit = false;
    private void InitRightRays()
    {
        if (_rightRayCount == 0)
            return;
        float height = Mathf.Abs(Vector2.Distance(_rightParentBottom.position, _rightParentTop.position));
        float raySpacing = height / _rightRayCount;
        for (int i = 0; i < _rightRayCount; ++i)
        {
            var _x = _rightRaysOffsetX;
            float _y = (i + 1) * raySpacing;
            _rightRayOffsets[i] = new Vector2(_x, _y); //replace the previous vector!
        }
    }
    private void CastRightRays()
    {
        if (_rightRayCount == 0)
            return;
        _rightRayHitBufferList.Clear();
        var origin = _rightParentBottom.position;
        Vector2 pos;
        int count = 0;
        RaycastHit2D hit;
        for (int i = 0; i < _rightRayCount; ++i)
        {
            pos = new(origin.x + _rightRayOffsets[i].x, origin.y + _rightRayOffsets[i].y);
            hit = Physics2D.Raycast(pos, Vector2.left, _leftRaysCastDistance, _layerMask);
            if(hit)
            {
                ++count;
                _rightRayHitBufferList.Add(hit);
            }
            if(i == 0)
                _bottomRightExtremeHit = hit;
            if(i == _leftRayCount - 1)
                _topRightExtremeHit = hit;
        }
        if (count == 0)
        {
            _rightCollisionType = CollisionTypes.NONE;
            return;
        }
        if (count == _rightRayCount)
        {
            _rightCollisionType = CollisionTypes.FULL;
            return;
        }
        _rightCollisionType = CollisionTypes.PARTIAL;
    }
   
    #endregion
}
