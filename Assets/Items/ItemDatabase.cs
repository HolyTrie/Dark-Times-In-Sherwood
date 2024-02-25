using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] public HealthPotionSO HealthPotion;

    public void SpawnItem(Transform spawnPos)
    {
        // Instantiate the item GameObject
        GameObject newItem = new GameObject("HealthPotion");
        newItem.transform.position = spawnPos.position;

        // Add SpriteRenderer component to the item GameObject
        SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();

        //Add rigidbody to item
        Rigidbody2D rigidbody2D = newItem.AddComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Add collision for IPickup
        BoxCollider2D boxCollider2D = newItem.AddComponent<BoxCollider2D>();
        // circleCollider2D.radius = 0.25f;
        boxCollider2D.size = new Vector2(0.475f,0.475f);


        // Set the sprite for the item
        if (HealthPotion != null && HealthPotion.ItemSprite != null)
        {
            spriteRenderer.sprite = HealthPotion.ItemSprite;
        }
        Vector3 itemDropOffset = new Vector3(0f,0.5f,0f);
        Instantiate(newItem, spawnPos.position+ itemDropOffset , Quaternion.identity);
    }
}
