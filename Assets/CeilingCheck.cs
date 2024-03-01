using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
    public enum CollisionTypes 
    {
        NONE,
        FULL,
        PARTIAL
    }
    public CollisionTypes CollisionType{get{return _collisionType;}}
    private const int _maxRays = 16;
    [Header("Ceiling Checks Details")]
    [SerializeField,Tooltip("Important - Only layers enabled by the layer mask will be checked against.")]
     private LayerMask _ceilLayer;
    [Header("Ray Cast properties")]
    [SerializeField] 
    private Transform _parentTopRight;
    [SerializeField] 
    private Transform _parentTopLeft;
    [SerializeField]
    private float _rayCastDistance = 1f;
    [SerializeField]
    private float _rayOffsetY = 0f;
    [SerializeField]
    private float _rayOffsetX = 0f;
    [SerializeField, Range(0,_maxRays)] 
    private int _rayCount = 5;
    protected List<RaycastHit2D> _rayHitBufferList = new(_maxRays);
    private CollisionTypes _collisionType = CollisionTypes.NONE;
    private readonly Vector2[] _rayOffsets = new Vector2[_maxRays];
    private void Awake()
    {
        InitRays();
    }

    private void InitRays()
    {
        if (_rayCount == 0) 
            return;
        float length = Mathf.Abs(Vector2.Distance(_parentTopLeft.position,_parentTopRight.position));
        float raySpacing = length / _rayCount;
        for(int i = 0 ; i<_rayCount ; ++i)
        {
            var _x = _rayOffsetX + (i+1) * raySpacing;
            var _y = _rayOffsetY;
            _rayOffsets[i] = new Vector2(_x,_y);
        }
    }
    private void FixedUpdate() {
        CastRays();
    }

    private void CastRays()
    {
        if(_rayCount == 0)
            return;
        _rayHitBufferList.Clear();
        var origin = _parentTopLeft.position;
        var facingRight = GameManager.PlayerIsFacingRight;
        Vector2 pos;
        for(int i = 0; i<_rayCount; ++i) // left to right 
        {
            pos = origin;
            pos.x += facingRight ? _rayOffsets[i].x : -_rayOffsets[i].x;
            pos.y += _rayOffsets[i].y;
            _rayHitBufferList.Add(Physics2D.Raycast(pos,Vector2.up,_rayCastDistance,_ceilLayer));
            // debug:
            var dest = new Vector2(pos.x,pos.y + _rayCastDistance);
            Debug.DrawLine(pos,dest,Color.red);
        }
        int count = 0;
        foreach(var hit in _rayHitBufferList)
        {
            if(!hit)
                continue;
            ++count;
        }
        if(count == 0)
        {
            _collisionType = CollisionTypes.NONE;
            return;
        }
        if(count == _rayCount)
        {
            _collisionType = CollisionTypes.FULL;
            return;
        }
        _collisionType = CollisionTypes.PARTIAL;
    }
    /*
    private void OnDrawGizmos()
    {
        var center = transform.position;
        center.x += _rayOffsetX;
        Vector3 vect = new(center.x, center.y + _rayCastDistance, center.z);
        Debug.DrawLine(start: center, end : vect, color : Color.white);
    }
    */
}
