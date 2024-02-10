using System.Collections;
using DTIS;
using UnityEngine;

public class GhostChestController : MonoBehaviour
{
    private Animator _animator;
    private PlayerStateMachine _playerFSM;
    private PlayerController _playerController;
    private bool guard = false;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        var col = GetComponent<SpriteRenderer>().color;
        col.b = 1f;
        col.g = 1f;
        col.r = 1f;
        col.a = 0.7f;
        GetComponent<SpriteRenderer>().color = col;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Floor"))
            return;
        
        //Debug.Log(guard +" ||| "+collider);
        if (!guard && collider.CompareTag("Player"))
        {
            _playerController = collider.gameObject.GetComponent<PlayerController>();
            if(_playerController.FSM.Controls.ActionMap.All.Interaction.IsPressed())
            {
                guard = true;
                _animator.Play("OpenChest");
                _playerController.Ghost();
                StartCoroutine(ReleaseGuard());
            }
        }
    }

    private IEnumerator ReleaseGuard(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        guard = false;
    }
}
