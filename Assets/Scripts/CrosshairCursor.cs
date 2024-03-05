using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    [SerializeField] private Texture2D BowSprite;
    [SerializeField] private Texture2D SwordSprite;
    public void BowCrosshair()
    {
        Cursor.SetCursor(BowSprite, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SwordCrosshair()
    {
        Cursor.SetCursor(SwordSprite, Vector2.zero, CursorMode.ForceSoftware);
    }
}
