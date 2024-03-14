using UnityEditor.UIElements;
using UnityEngine;
namespace DTIS
{
    /*sets the initial values of the spanwed object*/
    public class SpawnedObject : MonoBehaviour
    {
        [SerializeField] float force;
        [SerializeField] int DMG;
        [SerializeField] LayerMask Ground;
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
            if (other.tag == "Enemy") // Damage to enemies //other.gameObject.layer == 1 << 10
            {
                other.gameObject.GetComponent<HpBarEntity>().depleteHp(DMG);
                Destroy(this.gameObject);
            }

            if (other.gameObject.layer == 7) // ground layer
                Destroy(this.gameObject);
        }
    }
}
