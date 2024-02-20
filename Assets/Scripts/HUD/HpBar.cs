using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] public Slider hpBar;
    [SerializeField] private int maxHP = 100;
    
    private int currentHP;
    private WaitForSeconds regenTimer = new WaitForSeconds(0.1f);

    private Coroutine regen;

    public static HpBar instance; // used for later scenes

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
    }

    public void depleteHp(int amount)
    {
        Debug.Log("HITS PLAYER");
        currentHP -= amount;
        hpBar.value = currentHP;
    }

    public void restoreHp(int amount)
    {
        currentHP += amount;
        hpBar.value = currentHP;
    }

    public int currentHp()
    {
        return currentHP;
    }
}
