using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private LayerMask _layerMask;
    [Header("Right Ray Settings")]
    [SerializeField] private Transform _rightRayPosRef;
    [SerializeField] private float _rightRayCastDistance = 1f;
    [Header("Left Ray Settings")]
    [SerializeField] private Transform _leftRayPosRef;
    [SerializeField] private float _leftRayCastDistance = 1f;
    private Vector2 _rightRayPos;
    private Vector2 _leftRayPos;
    void Start()
    {
        _rightRayPos = new(_rightRayPosRef.position.x,_rightRayPosRef.position.y);
        _leftRayPos = new(_leftRayPosRef.position.x,_leftRayPosRef.position.y);
    }
    void Update()
    {
        _rightRayPos = new(_rightRayPosRef.position.x,_rightRayPosRef.position.y);
        _leftRayPos = new(_leftRayPosRef.position.x,_leftRayPosRef.position.y);
    }
    public bool SlopeAhead(bool _facingRight, float currY)
    {
        var ans = false;
        var pos = _facingRight ? _rightRayPos : _leftRayPos;
        var distance = _facingRight ? _rightRayCastDistance : _leftRayCastDistance;
        var hit = Physics2D.Raycast(pos,Vector2.down,distance,_layerMask);
        //Debug.DrawRay(pos,Vector2.down * distance, Color.cyan);
        if(hit)
        {
            var normalAngle = Vector2.Angle(hit.point,Vector2.up);
            normalAngle = Mathf.Abs(normalAngle);
            if(normalAngle > 0 && normalAngle < 1)
            {
                ans = true;
            }
        }
        return ans;
    }
    void OnDrawGizmos()
    {
        Vector2 rightRayPos = new(_rightRayPosRef.position.x,_rightRayPosRef.position.y);
        Vector2 leftRayPos = new(_leftRayPosRef.position.x,_leftRayPosRef.position.y);
        Debug.DrawRay(rightRayPos, Vector2.down*_rightRayCastDistance, Color.yellow);
        Debug.DrawRay(leftRayPos, Vector2.down*_leftRayCastDistance, Color.yellow);
    }
}
