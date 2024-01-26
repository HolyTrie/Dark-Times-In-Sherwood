using UnityEngine;
namespace DTIS
{

    /*This class is responsible for arrow tracking after he was shoot from the bow in order to make a good curve*/
    public class ArrowTracking : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        void FixedUpdate()
        {
            Vector2 direction = _rigidbody2D.velocity;
            //add angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // appllying the caluclated angle.
        }
    }
}
