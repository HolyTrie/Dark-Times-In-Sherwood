using System.Collections;
using DTIS;
using TMPro;
using UnityEngine;

public class BuffChestController : Interactable
{
    // Start is called before the first frame update
    // [SerializeField] private Item Item; // The item this chest gives the player.
    // [SerializeField] private float DestroyDelay;

    [Tooltip("This text will show up when chest opens to indicate buff given")]
    [SerializeField] private TextMeshProUGUI BuffText;

    [Tooltip("How long the buff lasts")]
    [SerializeField] private float BuffTime;

    [Tooltip("How strong the power it gives")]
    [SerializeField] private float JumpForceMultiplier;

    [Tooltip("How long text stays on screen")]
    [SerializeField] private float TextTime;

    //extra vars//
    private float prevForce;
    private bool guard = false;
    private PlayerStateMachine _playerFSM;
    private PlayerController _playerController;
    private Animator ChestAnimator;
    private int OneTimeBuff = 0;
    void Awake()
    {
        ChestAnimator = GetComponent<Animator>();
        _playerController = Util.GetPlayerController();
        BuffText.enabled = false;
    }

    // private IEnumerator DestoryChest()
    // {
    //     yield return new WaitForSeconds(DestroyDelay);

    //     // Destroy(this.gameObject);
    //     this.StartCoroutine(BuffLength());
    // }

    private IEnumerator JumpBuff() // Temporary jump boost
    {
        BuffText.text = "Temporary Super Jump";
        BuffText.enabled = true;
        prevForce = _playerController.JumpForce;
        _playerController.JumpForce *= JumpForceMultiplier;
        yield return new WaitForSeconds(TextTime);
        BuffText.enabled = false;
        yield return new WaitForSeconds(BuffTime);
        _playerController.JumpForce = prevForce;
        guard = false;
    }

    private IEnumerator HealthBuff() // perma health buff (this is very rare to happen)
    {
        BuffText.text = "5 Health Points Added";
        BuffText.enabled = true;
        int increaseHp = 5;
        _playerController.HpBar.UpdateMaxHP(increaseHp);
        yield return new WaitForSeconds(TextTime);
        BuffText.enabled = false;
    }
    private IEnumerator HealHP() // just heals the player
    {
        BuffText.text = "15 Health Points Restored";
        BuffText.enabled = true;
        int RestoreHP = 15;
        _playerController.HpBar.restoreHp(RestoreHP);
        yield return new WaitForSeconds(TextTime);
        BuffText.enabled = false;
    }

    private IEnumerator DMGBuff() //temporary damage buff - TBD
    {
        yield return new WaitForSeconds(TextTime);
    }


    private void BuffRandomizer()
    {
        int ChestItem = Random.Range(1, 3);
        Debug.Log("ChestItem:" + ChestItem);
        if (ChestItem == 1) // Jump buff
        {
            StartCoroutine(JumpBuff()); //released guard when its time
        }
        if (ChestItem == 2) //Perma Health bonus
        {
            StartCoroutine(HealthBuff());
        }
        if (ChestItem == 3)
        {
            StartCoroutine(HealHP());
        }
    }

    public override void OnClick(GameObject clickingEntity)
    {
        guard = true;
        if (OneTimeBuff == 0)
        {
            ChestAnimator.Play("OpenChest");
            BuffRandomizer();
            ++OneTimeBuff;
        }
    }
}
