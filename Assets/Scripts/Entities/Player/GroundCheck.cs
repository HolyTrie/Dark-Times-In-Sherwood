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
        [SerializeField] private Vector2 _groundCheckBoxSize;
        [SerializeField] private float _castDistance;
        [SerializeField] private LayerMask _groundLayer;
        private const int _downAngle = 0; //directly down from the origin.
        public bool Grounded()
        {
            var hit = Physics2D.BoxCast(transform.position,_groundCheckBoxSize,_downAngle,-transform.up,_castDistance,_groundLayer);
            if(hit)
                return true;
            return false;
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position-transform.up*_castDistance,_groundCheckBoxSize);
        }
    }
}
