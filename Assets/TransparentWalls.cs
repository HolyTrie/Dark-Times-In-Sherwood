using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWalls : MonoBehaviour
{
    [Header("Invisibility of tile")]
    [SerializeField, Range(0, 1)] private float alphaValue = 0.6f;
    public Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetAlpha();
    }

    private void SetAlpha()
    {
        var col = _renderer.material.color;
        col.a = alphaValue;
        _renderer.material.color = col;
    }
}
