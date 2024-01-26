using UnityEngine;

public class TileAntiGhostListener : MonoBehaviour{
    private TileGhostBehaviour _gb;
    private void Start() {
        _gb = new TileGhostBehaviour(GetComponent<Renderer>(),GetComponent<Collider2D>());
    }

    private void Update() {
        _gb.TrySetGhostStatus();
    }
    protected class TileGhostBehaviour : GhostBehaviour
        {
            public Renderer _renderer;
            public Collider2D _collider;
            public TileGhostBehaviour(Renderer renderer,Collider2D collider)
            {
                _renderer = renderer;
                _collider = collider;
            }
            protected override void OnGhostSet()
            {
                var col = _renderer.material.color;
                col.a = 0.9f;
                col.r = 255f;
                _renderer.material.color = col;
                _collider.isTrigger = false;
            }
            protected override void OnGhostUnset()
            {
                var col = _renderer.material.color;
                col.a = 0f;
                _renderer.material.color = col;
                _collider.isTrigger = true;
            }
        }
}