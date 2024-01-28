using UnityEngine;

public class TileGhostListener : MonoBehaviour{
    private TileGhostBehaviour _gb;
    [SerializeField] private bool _solidWhenGhost = true;
    [SerializeField, Range(0,1)] private float _alphaWhenGhost= 0.6f;
    [SerializeField, Range(0,1)] private float _alphaWhenNotGhost = 1f;
    private Renderer _renderer;
    private void Start() {
        _renderer = GetComponent<Renderer>();
        var colliders = new Collider2D[transform.childCount];
        int i = 0;
        foreach(Transform child in transform)
        {
            colliders[i] = child.gameObject.GetComponent<Collider2D>();
            ++i;
        }
        _gb = new TileGhostBehaviour(_renderer,colliders,_solidWhenGhost,_alphaWhenGhost,_alphaWhenNotGhost);
    }
    private void Update() {
        _gb.TrySetGhostStatus();
    }
    protected class TileGhostBehaviour : GhostBehaviour
        {
            public Renderer _renderer;
            public Collider2D[] _colliders;
            private bool _solidWhenGhost; 
            private float _alphaOnGhostSet;
            private float _alphaOnGhostUnset;
            public TileGhostBehaviour(Renderer renderer,Collider2D[] colliders,bool SolidWhenGhost,float AlphaOnGhostSet,float AlphaOnGhostUnset)
            {
                _renderer = renderer;
                _colliders = colliders;
                _solidWhenGhost = SolidWhenGhost;
                _alphaOnGhostSet = AlphaOnGhostSet;
                _alphaOnGhostUnset = AlphaOnGhostUnset;
            }
            protected override void OnGhostSet()
            {
                SetAlpha(_alphaOnGhostSet);
                foreach(var collider in _colliders)
                {
                    collider.isTrigger = !_solidWhenGhost;
                }
            }
            protected override void OnGhostUnset()
            {
                SetAlpha(_alphaOnGhostUnset);
                foreach(var collider in _colliders)
                {
                    collider.isTrigger = _solidWhenGhost;
                }
            }
            private void SetAlpha(float alphaValue)
            {
                var col = _renderer.material.color;
                col.a = alphaValue;
                _renderer.material.color = col;
            }
        }
}