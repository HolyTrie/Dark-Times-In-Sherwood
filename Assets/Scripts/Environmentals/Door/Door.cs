using System.Collections;
using DTIS;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool guard = false;
    private PlayerStateMachine _playerFSM;
    private PlayerController _playerController;
    private void OnTriggerStay2D(Collider2D collider)
    {
        
        _playerFSM = collider.gameObject.GetComponent<PlayerStateMachine>();
        _playerController = collider.gameObject.GetComponent<PlayerController>();
        if (!guard && collider.tag == "Player" && _playerFSM.Controls.ActionMap.All.Interaction.IsPressed())
        {
            guard = true;
            GameManager.NextScene();
            StartCoroutine(ReleaseGuard());
        }
    }

    private IEnumerator ReleaseGuard(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        guard = false;
    }
}
