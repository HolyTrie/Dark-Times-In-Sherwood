
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGhostBehaviour : GhostBehaviour
{
    [SerializeField] private readonly string _propertyNameInMaterial = "_Ghost";
    private const float _true = 1f;
    private const float _false = 0f;
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
        Debug.Log("SET");
        _renderer.material.SetFloat(_propertyNameInMaterial,_true);

        //sanity depletes/
        if(_sanityBar!=null)
            _sanityBar.UseSanity(_sanityCost);
        
    }
    protected override void OnGhostUnset()
    {
        //related to how the ghosted player looks//
        _renderer.material.SetFloat(_propertyNameInMaterial,_false);
        Debug.Log("UNSET");
        //sanity regen//
        if(_sanityBar!=null)
            _sanityBar.RegenSanity(_sanityCost);
    }
}