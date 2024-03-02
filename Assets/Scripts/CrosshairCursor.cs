using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    [SerializeField] private Sprite BowSprite;
    [SerializeField] private Sprite SwordSprite;
    private SpriteRenderer _cursorSprite;
    void Awake()
    {
        _cursorSprite = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
    }

    public void BowCrosshair()
    {
        _cursorSprite.sprite = BowSprite;
    }

    public void SwordCrosshair()
    {
        _cursorSprite.sprite = SwordSprite;
    }

    
}
