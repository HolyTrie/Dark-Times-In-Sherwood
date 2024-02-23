using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWorldPosition : MonoBehaviour
{
    private Vector3 _fixedPos;
    void Start()
    {
        _fixedPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _fixedPos;
    }
}
