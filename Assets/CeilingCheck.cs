using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
{
    [Header("Ceiling Checks Details")]
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private float _castDistance;
    [SerializeField] private float _offsetX;
    [SerializeField] private LayerMask _ceilLayer;
    private const int _downAngle = 180; //directly up from the origin.
    public bool Grounded()
    {
        Vector3 _vect = new(transform.position.x + _offsetX, transform.position.y, transform.position.z);
        var hit = Physics2D.BoxCast(_vect, _boxSize, _downAngle, -transform.up, _castDistance, _ceilLayer);
        if (hit)
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 _center = transform.position - transform.up * _castDistance;
        Vector3 _vect = new(_center.x + _offsetX, _center.y, _center.z);

        Gizmos.DrawWireCube(_vect, _boxSize);;
    }
}
