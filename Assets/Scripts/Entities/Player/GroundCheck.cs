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
        [Header("Ray 1")]
        [SerializeField] private Vector2 _ray1Size;
        [SerializeField] private float  _ray1OffsetX;
        [SerializeField] private float _ray1CastDistance;
        [Header("Ray 2")]
        [SerializeField] private Vector2 _ray2Size;
        [SerializeField] private float  _ray2OffsetX;
        [SerializeField] private float _ray2CastDistance;
        
        [Header("Common Parameters")]
        
        [SerializeField] private LayerMask _groundLayer;
        public void SetLayer(LayerMask layerMask)
        {
            _groundLayer = layerMask;
        }
        private const int _downAngle = 0; //directly down from the origin.
        public bool Grounded()
        {
            var ans = false;
            Vector3 pos1 = new(transform.position.x + _ray1OffsetX,transform.position.y,transform.position.z);
            Vector3 pos2 = new(transform.position.x + _ray2OffsetX,transform.position.y,transform.position.z);
            var hitOne = Physics2D.BoxCast(pos1,_ray1Size,_downAngle,-(Vector3)Vector2.up,_ray1CastDistance,_groundLayer);
            var hitTwo = Physics2D.BoxCast(pos2,_ray2Size,_downAngle,-(Vector3)Vector2.up,_ray2CastDistance,_groundLayer);
            if(hitOne && hitTwo)
                ans = true;
            return ans;
        }

        private void OnDrawGizmos() {
            Vector3 center1 = transform.position - (Vector3)Vector2.up * _ray1CastDistance;
            Vector3 center2 = transform.position - (Vector3)Vector2.up * _ray2CastDistance;
            Vector3 two = new(center1.x + _ray1OffsetX,center1.y,center1.z);
            Vector3 four = new(center2.x + _ray2OffsetX,center2.y,center2.z);
            Gizmos.DrawWireCube(two,_ray1Size);
            Gizmos.DrawWireCube(four,_ray2Size);
        }
    }
}
