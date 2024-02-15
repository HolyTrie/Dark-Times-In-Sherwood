using DTIS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider2D), typeof(TextMeshProUGUI))]
public sealed class ProximityMessage : MonoBehaviour
{
    private TextMeshProUGUI _textGUI;
    private Collider2D _collider;
    private GameObject _player;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _textGUI = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        _textGUI.enabled = false;
    }
    private void Update()
    {
        if (_player != null)
        {
            /*
            var distance = Vector2.Distance(_player.transform.position,transform.position);
            var distanceFraction = distance/_collider.bounds.size.x/2;
            Debug.Log(distance);
            Debug.Log(_collider.bounds.size.x);
            Debug.Log(distanceFraction);
            _textGUI.alpha = distanceFraction;
            */
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            _player = other.gameObject;
        _textGUI.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player = null;
        _textGUI.enabled = false;
    }
}