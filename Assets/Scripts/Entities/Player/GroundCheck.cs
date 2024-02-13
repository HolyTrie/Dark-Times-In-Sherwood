using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS
{
    /// <summary>
    /// Wrapper class for raycast ground checks, attach this to your 'controller' or equivalent.
    /// </summary>
    public class GroundCheck : MonoBehaviour
    {
        /*
        private bool _grounded = true;
        public bool Grounded { get { return _grounded; } set { _grounded = value; } }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Floor") || other.CompareTag("Platform"))
                _grounded = true;
        }
        private void OnTriggerExit2D()
        {
            _grounded = false;
        }
        */
        [SerializeField] private Vector2 _leftBoxSize;
        [SerializeField] private float _leftBoxOffsetX;
        [SerializeField] private Vector2 _rightBoxSize;
        [SerializeField] private float _rightBoxOffsetX;
        [SerializeField] private float _castDistance;
        [SerializeField] private LayerMask _groundLayer;
        private const int _downAngle = 0; //directly down from the origin.
        public bool Grounded()
        {
            Vector3 _left = new(transform.position.x - _leftBoxOffsetX,transform.position.y,transform.position.z);
            Vector3 _right = new(transform.position.x + _rightBoxOffsetX,transform.position.y,transform.position.z);
            var hitLeft = Physics2D.BoxCast(_left,_leftBoxSize,_downAngle,-transform.up,_castDistance,_groundLayer);
            var hitRight = Physics2D.BoxCast(_right,_rightBoxSize,_downAngle,-transform.up,_castDistance,_groundLayer);
            if(hitLeft && hitRight)
                return true;
            return false;
        }

        private void OnDrawGizmos() {
            Vector3 _center = transform.position - transform.up * _castDistance;
            Vector3 _left = new(_center.x - _leftBoxOffsetX,_center.y,_center.z);
            Vector3 _right = new(_center.x + _rightBoxOffsetX,_center.y,_center.z);

            Gizmos.DrawWireCube(_left,_leftBoxSize);
            Gizmos.DrawWireCube(_right,_rightBoxSize);
        }
    }
}
