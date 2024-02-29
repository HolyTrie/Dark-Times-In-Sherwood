using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
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
    //protected List<RaycastHit2D> _rayHitBufferList = new(_maxRays);
    private readonly Vector2[] _rayOffsets = new Vector2[_maxRays];
    private int _leftToRightCollisionCount = 0;
    private int _rightToLeftCollisionCount = 0;
    public int LeftToRightCollisionCount { get { return _leftToRightCollisionCount; } }
    public int RightToLeftCollisionCount { get { return _rightToLeftCollisionCount; } }
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
            _rayOffsets[i] = new Vector2(_x,_y); //replace the previous vector!
        }
    }
    private void FixedUpdate() {
        CastRays();
    }

    private void CastRays()
    {
        var origin = _parentTopLeft.position;
        Vector2 pos;
        RaycastHit2D hit;
        var rightToLeftCollisionCount = 0;
        var leftToRightCollisionCount = 0;
        for(int i =0; i<_rayCount; ++i) // left to right count
        {
            pos = new(origin.x+_rayOffsets[i].x,origin.y+_rayOffsets[i].y);
            var dest = new Vector2(pos.x,pos.y + _rayCastDistance);
            hit = Physics2D.Raycast(pos,Vector2.up,_rayCastDistance,_ceilLayer);
            Debug.DrawLine(pos,dest,Color.red);
            if(!hit)
            {
                break;
            }
            ++leftToRightCollisionCount;
        }
        origin = _parentTopRight.position;
        for(int i = _rayCount-1; i >= 0; --i) // right to left count
        { 
            pos = new(origin.x+_rayOffsets[i].x,origin.y+_rayOffsets[i].y);
            var dest = new Vector2(pos.x,pos.y + _rayCastDistance);
            hit = Physics2D.Raycast(pos,Vector2.up,_rayCastDistance,_ceilLayer);
            Debug.DrawLine(pos,dest,Color.blue);
            if(!hit)
            {
                break;
            }
            ++rightToLeftCollisionCount;
        }
        _rightToLeftCollisionCount = rightToLeftCollisionCount;
        _leftToRightCollisionCount = leftToRightCollisionCount;
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
