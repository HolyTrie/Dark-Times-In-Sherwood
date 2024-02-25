using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Health Potion", order = 1)]

public class HealthPotionSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    [SerializeField] public string potionName;
    [SerializeField] public Sprite ItemSprite;

    [Header("Rewards")]
    [SerializeField] public int healingAmount;


    // ensures the id is always the name of the Scriptable object asset.
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}