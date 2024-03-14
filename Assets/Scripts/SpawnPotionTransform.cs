using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPotionTransform : MonoBehaviour
{
    void Start()
    {
        this.transform.GetComponent<ItemDatabase>().HealthPotion.SpawnItem(this.transform);
    }
}
