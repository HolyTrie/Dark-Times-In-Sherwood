using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public enum OneWayPlatforms { GoingUp, GoingDown, Both }
    public OneWayPlatforms Type => _type;
    public Collider2D Collider => _collider;
    [SerializeField] private OneWayPlatforms _type = OneWayPlatforms.Both;
    private Collider2D _collider;
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
}
