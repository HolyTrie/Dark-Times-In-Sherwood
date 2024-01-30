using System.Collections;
using DTIS;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private Item Item; // The item this chest gives the player.
    // [SerializeField] private float DestroyDelay;
    [SerializeField] private float BuffTime; // Temporary 
    [SerializeField] private float JumpForceBuff; // Temporary
    private Animator _animator;
    private PlayerStateMachine _playerFSM;
    private PlayerController _playerController;
    void Awake()
    {
        _animator = this.GetComponent<Animator>();

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        _playerFSM = collider.gameObject.GetComponent<PlayerStateMachine>();
        _playerController = collider.gameObject.GetComponent<PlayerController>();
        if (collider.tag == "Player" && _playerFSM.Controls.ActionMap.All.Interaction.IsPressed())
        {
            _animator.Play("OpenChest");
            Debug.Log("On platform");
            this.StartCoroutine(BuffLength());
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
        Debug.Log("JumpForce:"+_playerController.JumpForce);
        _playerController.JumpForce = JumpForceBuff;
        yield return new WaitForSeconds(BuffTime);
        _playerController.JumpForce = 15f;
        Debug.Log("JumpForce:"+_playerController.JumpForce);
    }
}
