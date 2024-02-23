using System;
using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class SawTrapCollider : MonoBehaviour
{
    [SerializeField] private int AttackDMG;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().HpBar.depleteHp(AttackDMG);

        }
    }
}
