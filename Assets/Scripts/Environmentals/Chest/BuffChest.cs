using System.Collections;
using DTIS;
using UnityEngine;

public class BuffChestController : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private Item Item; // The item this chest gives the player.
    // [SerializeField] private float DestroyDelay;
    [SerializeField] private float BuffTime;
    [SerializeField] private float JumpForceMultiplier;
    private float prevForce;
    private bool guard = false;
    private Animator _animator;
    private PlayerStateMachine _playerFSM;
    private PlayerController _playerController;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _playerFSM = collider.gameObject.GetComponent<PlayerStateMachine>();
        _playerController = collider.gameObject.GetComponent<PlayerController>();
        if (!guard && collider.tag == "Player" && _playerFSM.Controls.ActionMap.All.Interaction.IsPressed())
        {
            guard = true;
            _animator.Play("OpenChest");
            Debug.Log("On platform");
            StartCoroutine(BuffLength());
        }
    }
    // private IEnumerator DestoryChest()
    // {
    //     yield return new WaitForSeconds(DestroyDelay);

    //     // Destroy(this.gameObject);
    //     this.StartCoroutine(BuffLength());
    // }

    private IEnumerator BuffLength() // Temporary
    {
        //Debug.Log("JumpForce:"+_playerController.JumpForce);
        prevForce = _playerController.JumpForce;
        _playerController.JumpForce *= JumpForceMultiplier;
        yield return new WaitForSeconds(BuffTime);
        _playerController.JumpForce = prevForce;
        guard = false;
        //Debug.Log("JumpForce:"+_playerController.JumpForce);
    }
}
