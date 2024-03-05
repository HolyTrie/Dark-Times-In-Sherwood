using System.Collections;
using System.Collections.Generic;
using DTIS;
using Unity.VisualScripting;
using UnityEngine;

public class HpPickUp : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerController pc = Util.GetPlayerController();
            int healingAmount = pc.GetComponent<ItemDatabase>().HealthPotion.healingAmount;
            if (pc.HpBar.currentHp() + healingAmount > pc.HpBar.MaxHp()) // incase healing is above the maxHP
            {
                pc.HpBar.HealToFull();
            }
            else
                pc.HpBar.restoreHp(healingAmount);

            Destroy(this.gameObject);
        }
    }
}
