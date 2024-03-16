using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarPlayer : MonoBehaviour
{
    [SerializeField] public Slider hpBar;
    [SerializeField] private static int maxHP = 120;
    private TextMeshProUGUI TextHP;
    private static int currentHP = maxHP; // starters
    private WaitForSeconds regenTimer = new WaitForSeconds(0.1f);

    private Coroutine regen;

    public static HpBarPlayer instance; // used for later scenes

    private void Awake()
    {
        instance = this;
        // currentHP = maxHP;
    }

    void Start()
    {
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        TextHP = hpBar.transform.Find("Points").GetComponent<TextMeshProUGUI>();
        TextHP.text = currentHP + "/" + maxHP;
    }

    public void depleteHp(int amount)
    {
        // Debug.Log("HITS PLAYER");
        currentHP -= amount;
        hpBar.value = currentHP;

        TextHP.text = currentHP + "/" + maxHP;

        if(currentHP <= 0) // player is dead -> this could be done better i guess
        {
            HealToFull();
            GameManager.ResetScene();
        }
    }

    public void restoreHp(int amount)
    {
        currentHP += amount;
        hpBar.value = currentHP;

        TextHP.text = currentHP + "/" + maxHP;
    }

    public int currentHp()
    {
        return currentHP;
    }

    public void HealToFull()
    {
        currentHP = maxHP;
        hpBar.value = currentHP;

        TextHP.text = currentHP + "/" + maxHP;
    }

    //this can be updated if the player upgrades 
    public int MaxHp()
    {
        return maxHP;
    }

    public void UpdateMaxHP(int HPBonus)
    {
        maxHP += HPBonus;
        
        TextHP.text = currentHP + "/" + maxHP;
    }

    public void ResetToMax()
    {
        currentHP = maxHP;
    }
}
