using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]

public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    [SerializeField] public string displayName;

    [Header("Requirements")]
    [SerializeField] public int levelRequirement;
    [SerializeField] public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    [SerializeField] GameObject[] questStepPrefabs;

    [Header("Rewards")]
    [SerializeField] public int goldReward;
    [SerializeField] public int experienceReward;


    // ensures the id is always the name of the Scriptable object asset.
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}