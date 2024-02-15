using System.Collections;
using UnityEngine;

/**
 * This component spawns the given object whenever the player clicks a given key.
 */
namespace DTIS
{
    public class ClickSpawn : MonoBehaviour
    {
        [SerializeField] protected GameObject prefabToSpawn;
        [SerializeField] Transform spawnPosition;
        [SerializeField] private float _timeToLiveSeconds = 10f;
        private Vector3 mouseWorldPosition;
        private Camera _mainCamera;
        void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        
        /* This method generates an object according to the mouse cursor position */
        public void spawnObject()
        {
            //Debug.Log("Spawning a new object");
            mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Set positions//
            Vector3 rotation = mouseWorldPosition - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            var go = Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
            StartCoroutine(Util.DestroyGameObjectCountdown(go,_timeToLiveSeconds));
        }
    }
}