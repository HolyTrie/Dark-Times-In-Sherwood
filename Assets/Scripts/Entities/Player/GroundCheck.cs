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
        [Header("Ray 1")]
        [SerializeField] private Vector2 _ray1Size;
        [SerializeField] private float  _ray1OffsetX;
        [SerializeField] private float _ray1CastDistance;
        [Header("Ray 2")]
        [SerializeField] private Vector2 _ray2Size;
        [SerializeField] private float  _ray2OffsetX;
        [SerializeField] private float _ray2CastDistance;
        [Header("Ray 3")]
        [SerializeField] private Vector2 _ray3Size;
        [SerializeField] private float  _ray3OffsetX;
        [SerializeField] private float _ray3CastDistance;
        
        [Header("Common Parameters")]
        
        [SerializeField] private LayerMask _groundLayer;
        private const int _downAngle = 0; //directly down from the origin.
        public bool Grounded()
        {
            Vector3 pos1 = new(transform.position.x + _ray1OffsetX,transform.position.y,transform.position.z);
            Vector3 pos2 = new(transform.position.x + _ray2OffsetX,transform.position.y,transform.position.z);
            Vector3 pos3 = new(transform.position.x + _ray3OffsetX,transform.position.y,transform.position.z);
            var hitOne = Physics2D.BoxCast(pos1,_ray1Size,_downAngle,-transform.up,_ray1CastDistance,_groundLayer);
            var hitTwo = Physics2D.BoxCast(pos2,_ray2Size,_downAngle,-transform.up,_ray2CastDistance,_groundLayer);
            var hitThree = Physics2D.BoxCast(pos3,_ray3Size,_downAngle,-transform.up,_ray3CastDistance,_groundLayer);
            if(hitOne && hitTwo && hitThree)
                return true;
            return false;
        }

        private void OnDrawGizmos() {
            Vector3 center1 = transform.position - transform.up * _ray1CastDistance;
            Vector3 center2 = transform.position - transform.up * _ray2CastDistance;
            Vector3 center3 = transform.position - transform.up * _ray3CastDistance;
            Vector3 one = new(center1.x + _ray1OffsetX,center1.y,center1.z);
            Vector3 two = new(center2.x + _ray2OffsetX,center2.y,center2.z);
            Vector3 three = new(center3.x + _ray3OffsetX,center3.y,center3.z);

            Gizmos.DrawWireCube(one,_ray1Size);
            Gizmos.DrawWireCube(two,_ray2Size);
            Gizmos.DrawWireCube(three,_ray3Size);
        }
    }
}
