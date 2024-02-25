using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] public HealthPotionSO HealthPotion;
    public void SpawnItem(Transform spawnPos)
    {
        // Instantiate the item GameObject
        Vector3 itemDropOffset = new Vector3(0f,0.5f,0f);
        GameObject newItem = new GameObject("HealthPotion");
        newItem.transform.position = spawnPos.position + itemDropOffset;

        // Add SpriteRenderer component to the item GameObject
        SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();

        //Add rigidbody to item
        Rigidbody2D rigidbody2D = newItem.AddComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Add collision
        BoxCollider2D boxCollider2D = newItem.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(0.475f,0.475f);

        //Add collision for IPickup
        CircleCollider2D circleCollider2D = newItem.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = 0.45f;
        circleCollider2D.isTrigger = true;

        //Add IPickUP
        PickUp pickUp = newItem.AddComponent<PickUp>();

        // Set the sprite for the item
        if (HealthPotion != null && HealthPotion.ItemSprite != null)
        {
            spriteRenderer.sprite = HealthPotion.ItemSprite;
        }
    }
}
