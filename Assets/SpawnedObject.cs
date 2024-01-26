using UnityEngine;
namespace DTIS
{
    /*sets the initial values of the spanwed object*/
    public class SpawnedObject : MonoBehaviour
    {
        [SerializeField] float force;
        private Vector3 mouseWorldPosition;
        private Camera _mainCamera;
        private Rigidbody2D _rigidbody2D;

        void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Set positions//
            Vector3 direction = mouseWorldPosition - transform.position;
            Vector3 rotation = transform.position - mouseWorldPosition;
            //add velocity
            _rigidbody2D.velocity = new Vector2(direction.x, direction.y).normalized * force;
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // appllying the caluclated angle.
        }
        /*if arrow hits floor it will destroy on impact (maybe do this better, but for now will suffice)*/
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Floor")
            {
                // Debug.Log("Arrow Destroyed");
                Destroy(this.gameObject);
            }

        }
    }
}
