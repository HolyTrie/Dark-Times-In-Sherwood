using System.Collections;
using DTIS;
using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private Collider2D _collider2d;
    private float _wait = 0.5f;

    private void Start()
    {
        _collider2d = gameObject.GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("On platform");
            _collider2d.isTrigger = false; // after jumping on the platform set the trigger to false.
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log(collision.gameObject.name);
            PlayerStateMachine _player = collision.gameObject.GetComponent<PlayerController>().FSM;
            if (_player.Controls.ActionMap.All.Down.IsPressed())
            {
                //Debug.Log("Player Going Down");
                _collider2d.isTrigger = true; // after leaving it / pressing down to jump set it to true, so the player can go through it down.
                StartCoroutine(DisableTrigger()); // disalbe trigger after player has passed it.
            }
        }
    }
    private IEnumerator DisableTrigger()
    {
        yield return new WaitForSeconds(_wait);
        _collider2d.isTrigger = false;
    }
}
