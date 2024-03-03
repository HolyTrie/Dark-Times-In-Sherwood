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
        InitRays();
    }
    void FixedUpdate()
    {
        CastRays();
    }

    [Header("General Setting (Applies to both sides)")]

    private const int _maxRays = 16;
    [Tooltip("Important - Only layers enabled by the layer mask will be checked against.")]
    [SerializeField] protected private LayerMask _layerMask; 
    public bool FirstFromTopHit { get { return _firstFromTopHit; } }
    public bool SecondFromTopHit { get { return _secondFromTopHit; } }
    public bool FirstFromBottomHit { get { return _firstFromBottomHit; } }
    public CollisionTypes CollisionType { get { return _collisionType; } }

    [Header("Right side raycasts settings")]
    [SerializeField] 
    private Transform _parentTop;
    [SerializeField] 
    private Transform _parentBottom;
    [SerializeField, Range(0, _maxRays)] 
    private int _rayCount = 5;
    [SerializeField] 
    private float _raysOffsetX;
    [SerializeField] 
    private float _raysCastDistance;
    private readonly Vector2[] _rayOffsets = new Vector2[_maxRays];
    protected private CollisionTypes _collisionType = CollisionTypes.NONE;
    protected private List<RaycastHit2D> _rayHitBufferList = new(_maxRays);
    private bool _firstFromTopHit = false;
    private bool _secondFromTopHit = false;
    private bool _firstFromBottomHit = false;
    private void InitRays()
    {
        if (_rayCount == 0)
            return;
        float height = Mathf.Abs(Vector2.Distance(_parentBottom.position, _parentTop.position));
        float raySpacing = height / _rayCount;
        for (int i = 0; i < _rayCount; ++i)
        {
            var _x = _raysOffsetX;
            float _y = (i + 1) * raySpacing;
            _rayOffsets[i] = new Vector2(_x, _y); //replace the previous vector!
        }
    }
    private void CastRays()
    {
        if (_rayCount == 0)
            return;
        _rayHitBufferList.Clear();
        var origin = _parentBottom.position;
        Vector2 pos;
        int count = 0;
        RaycastHit2D hit;
        var direction = GameManager.PlayerIsFacingRight ? Vector2.right : Vector2.left;
        var length = _rayCount - 1;
        for (int i = 0; i <= length; ++i)
        {
            pos = new(origin.x + _rayOffsets[i].x, origin.y + _rayOffsets[i].y);
            hit = Physics2D.Raycast(pos, direction, _raysCastDistance, _layerMask);
            Debug.DrawLine(pos,new(pos.x+direction.x,pos.y),Color.blue);
            if(hit)
            {
                ++count;
                _rayHitBufferList.Add(hit);
            }
            if(i == 0)
                _firstFromBottomHit = hit;
            if(i == length - 1)
                _secondFromTopHit = hit;
            if(i == length)
                _firstFromTopHit = hit;
        }
        //Debug.Log($"first from top hit = {_firstFromTopHit} | 2nd from top hit = {_secondFromTopHit}");
        if (count == 0)
        {
            _collisionType = CollisionTypes.NONE;
            return;
        }
        if (count == _rayCount)
        {
            _collisionType = CollisionTypes.FULL;
            return;
        }
        _collisionType = CollisionTypes.PARTIAL;
    }
}
