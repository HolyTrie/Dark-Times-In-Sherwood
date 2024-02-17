using System;
using System.Collections;
using DTIS;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GateController : MonoBehaviour
{
    [Tooltip("Delay between segments to open the gate")]
    [SerializeField] private float _GateSegmentOpenDelaySeconds = 0.25f;

    private SpriteRenderer[] _SpriteChilds;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _SpriteChilds = new SpriteRenderer[transform.childCount];

        for (int i = 0; i < _SpriteChilds.Length; ++i)
        {
            _SpriteChilds[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }

    }

    public void OpenGate()
    {
        StartCoroutine(OpenOneByOne(_GateSegmentOpenDelaySeconds));
    }

    private IEnumerator OpenOneByOne(float delay)
    {
        for (int i = _SpriteChilds.Length - 1; i >= 0; --i)
        {
            yield return new WaitForSeconds(delay);
            if (i != 0)
                _SpriteChilds[i].transform.position = Vector3.Lerp(_SpriteChilds[i].transform.position, _SpriteChilds[i - 1].transform.position, 0.5f);
            _SpriteChilds[i].enabled = false;
        }
        _collider.enabled = false; //finally turn off the collision.
    }
}
