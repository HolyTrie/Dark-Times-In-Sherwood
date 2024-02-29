using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
    [Header("Ceiling Checks Details")]
    [SerializeField] private LayerMask _ceilLayer;
    [Header("Ray Cast properties")]
    [SerializeField]
    private float _rayCastDistance = 1f;
    [SerializeField]
    private float _rayOffsetX = 0f;
    [Header("Box Cast properties")]
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private float _boxCastDistance;
    [SerializeField] private float _boxOffsetX;
    private const int _downAngle = 180; //directly up from the origin.
    private void FixedUpdate() {
        Vector3 origin = new(transform.position.x + _rayOffsetX, transform.position.y, transform.position.z);
        var hit = Physics2D.Raycast(origin, Vector2.up,_rayCastDistance,_ceilLayer);
        if (hit)
            Debug.Log(hit.transform.name);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position + transform.up * _boxCastDistance;
        Vector3 vect = new(center.x + _boxOffsetX, center.y, center.z);
        Gizmos.DrawWireCube(vect, _boxSize);
        center = transform.position;
        center.x += _rayOffsetX;
        vect = new(center.x, center.y + _rayCastDistance, center.z);
        Debug.DrawLine(start: center, end : vect, color : Color.white);
    }
}
