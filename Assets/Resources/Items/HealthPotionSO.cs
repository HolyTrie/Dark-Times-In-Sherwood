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

    [Header("Physics")]
    [SerializeField] public float PickUpRadius;
    [SerializeField] public Vector2 BoxCollderSize;

    public void SpawnItem(Transform spawnPos)
    {
        // Instantiate the item GameObject
        Vector3 itemDropOffset = new Vector3(0f, 0.5f, 0f);
        GameObject newItem = new GameObject(potionName);
        newItem.transform.position = spawnPos.position + itemDropOffset;

        // Add SpriteRenderer component to the item GameObject
        SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();

        //Add rigidbody to item
        Rigidbody2D rigidbody2D = newItem.AddComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Add collision
        BoxCollider2D boxCollider2D = newItem.AddComponent<BoxCollider2D>();
        boxCollider2D.size = BoxCollderSize;

        //Add collision for IPickup
        CircleCollider2D circleCollider2D = newItem.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = PickUpRadius;
        circleCollider2D.isTrigger = true;

        //Add IPickUP
        HpPickUp pickUp = newItem.AddComponent<HpPickUp>();

        // Set the sprite for the item
        if (ItemSprite != null)
        {
            spriteRenderer.sprite = ItemSprite;
        }
    }

    // ensures the id is always the name of the Scriptable object asset.
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}