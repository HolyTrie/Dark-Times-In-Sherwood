
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGhostBehaviour : GhostBehaviour
{
    private readonly Renderer _renderer;
    private SanityBar _sanityBar;
    private int _sanityCost = 5;

    public PlayerGhostBehaviour(SpriteRenderer renderer, SanityBar sanityBar, int sanityCost)
    {
        _renderer = renderer;
        _sanityBar = sanityBar;
        _sanityCost = sanityCost;
        
    }
    protected override void OnGhostSet()
    {
        //related to how the ghosted player looks//
        var col = _renderer.material.color;
        col.a = 0.5f;
        _renderer.material.color = col;

        //sanity depletes//
        _sanityBar.UseSanity(_sanityCost);
        
    }
    protected override void OnGhostUnset()
    {
        //related to how the ghosted player looks//
        var col = _renderer.material.color;
        col.a = 1f;
        _renderer.material.color = col;

        //sanity regen//
        _sanityBar.RegenSanity(_sanityCost);
    }
}