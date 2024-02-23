using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawPressurePlate : MonoBehaviour
{
    [SerializeField] SawTrapController SawTrap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
            SawTrap.StartSaw();
    }
}
