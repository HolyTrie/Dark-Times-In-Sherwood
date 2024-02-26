using DTIS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider2D), typeof(Canvas))]
public sealed class ProximityMessage : MonoBehaviour
{
    private Canvas _canvas;
    private Collider2D _collider;
    private GameObject _player;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        _canvas.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            _player = other.gameObject;
        _canvas.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player = null;
        _canvas.enabled = false;
    }
}