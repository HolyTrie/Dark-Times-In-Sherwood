using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPressurePlate : MonoBehaviour
{
    [SerializeField] TrapController Trap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
            Trap.StartTrap();
    }
    
}
