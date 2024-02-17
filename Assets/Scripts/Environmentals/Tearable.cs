using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tearable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
            Destroy(this.gameObject);

    }
}
