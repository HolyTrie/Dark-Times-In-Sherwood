using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    [SerializeField] private Texture2D BowSprite;
    [SerializeField] private Texture2D SwordSprite;

    private SpriteRenderer _cursorSprite;
    
    void Awake()
    {
        // _cursorSprite = GetComponent<SpriteRenderer>();
        // Cursor.visible = false;
    }

    void Update()
    {
        // Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // transform.position = mouseCursorPos;
    }

    public void BowCrosshair()
    {
        // _cursorSprite.sprite = BowSprite;
        Cursor.SetCursor(BowSprite,Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SwordCrosshair()
    {
        // _cursorSprite.sprite = SwordSprite;
        Cursor.SetCursor(SwordSprite,Vector2.zero, CursorMode.ForceSoftware);
    }

    
}
