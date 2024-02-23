using UnityEngine;

public class TileGhostListener : MonoBehaviour
{
    [Header("Triggers")]
    [SerializeField] private bool startsSolid = false;
    [SerializeField] private bool solidWhenGhost = true;

    [Header("Invisibility of tile")]
    [SerializeField, Range(0, 1)] private float alphaWhenGhost = 0.6f;
    [SerializeField, Range(0, 1)] private float alphaWhenNotGhost = 1f;

    private TileGhostBehaviour _gb;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        var colliders = GetComponent<Collider2D>();
        _gb = new TileGhostBehaviour(_renderer, colliders, startsSolid, solidWhenGhost, alphaWhenGhost, alphaWhenNotGhost);
    }
    private void Update()
    {
        _gb.TrySetGhostStatus();
    }
    protected class TileGhostBehaviour : GhostBehaviour
    {
        public Renderer _renderer;
        public Collider2D _colliders;
        private bool _solidWhenGhost;
        private bool _initialGhosSet;
        private float _alphaOnGhostSet;
        private float _alphaOnGhostUnset;
        public TileGhostBehaviour(Renderer renderer, Collider2D colliders, bool startsGhosted, bool SolidWhenGhost, float AlphaOnGhostSet, float AlphaOnGhostUnset)
        {
            _renderer = renderer;
            _colliders = colliders;
            _solidWhenGhost = SolidWhenGhost;
            _alphaOnGhostSet = AlphaOnGhostSet;
            _alphaOnGhostUnset = AlphaOnGhostUnset;
            _initialGhosSet = startsGhosted;
            if (_initialGhosSet) { OnGhostSet(); }
            else { OnGhostUnset(); }
        }
        protected override void OnGhostSet()
        {
            SetAlpha(_alphaOnGhostSet);
            _colliders.isTrigger = !_solidWhenGhost;

        }
        protected override void OnGhostUnset()
        {
            SetAlpha(_alphaOnGhostUnset);
            _colliders.isTrigger = _solidWhenGhost;

        }
        private void SetAlpha(float alphaValue)
        {
            var col = _renderer.material.color;
            col.a = alphaValue;
            _renderer.material.color = col;
        }
    }
}