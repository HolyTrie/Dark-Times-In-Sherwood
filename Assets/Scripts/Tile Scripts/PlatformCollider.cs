using System.Collections;
using DTIS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlatformCollider : MonoBehaviour
{
    private Collider2D _collider2d;
    [SerializeField] private float _wait = 0.5f;

    private void Start()
    {
        _collider2d = gameObject.GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var contactPoint = collision.GetContact(0).point;
            var dir = collision.transform.position - (Vector3)contactPoint;
            dir = collision.transform.InverseTransformDirection(dir);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bool below_platform = angle < 0; 

            if(below_platform)
            {
                Debug.Log("from below");
                _collider2d.enabled = false;
                StartCoroutine(WaitThenEnable(_wait));
            }
            else
            {
                Debug.Log("from above");
                _collider2d.enabled = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            
            Debug.Log(collision.gameObject.name);
            PlayerStateMachine _player = collision.gameObject.GetComponent<PlayerController>().FSM;
            if (_player.Controls.ActionMap.All.Down.IsPressed())
            {
                //Debug.Log("Player Going Down");
                _collider2d.enabled = false; 
                StartCoroutine(WaitThenEnable(_wait));
            }
        }
    }
    private IEnumerator WaitThenEnable(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _collider2d.enabled = true;
    }
}
