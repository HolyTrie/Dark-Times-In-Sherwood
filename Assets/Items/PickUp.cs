using System.Collections;
using System.Collections.Generic;
using DTIS;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerController pc = Util.GetPlayerController();
            pc.HpBar.restoreHp(pc.GetComponent<ItemDatabase>().HealthPotion.healingAmount);
            Destroy(this.gameObject);
        }
    }
}
