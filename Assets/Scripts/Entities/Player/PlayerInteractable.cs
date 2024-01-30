using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private HashSet<Collider2D> colliders = new HashSet<Collider2D>();
    private Collider2D closest_collider;
    [SerializeField] Collider2D PlayerInteractCollider;
    private Collider2D highlighted_collider;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        highlighted_collider = FindClosestCollider();
    }

    private Collider2D FindClosestCollider()
    {
        Collider2D closest_collider = null;
        float closest_distance = float.MaxValue;
        float distance = 0;
        foreach (var collider in colliders)
        {
            if (closest_collider = null)
            {
                closest_collider = collider;
                distance = Physics2D.Distance(PlayerInteractCollider, collider).distance;
            }
            else
            {
                distance = Physics2D.Distance(PlayerInteractCollider, collider).distance;
                if (distance < closest_distance)
                {
                    closest_collider = collider;
                    closest_distance = distance;
                }
            }
        }
        return closest_collider;
    }

    void CheckInteractables()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Interactable")
        {
            colliders.Add(collider);
        }
    }
    void OnTriggerExit2D(Collider2D coll) { colliders.Remove(coll); }
}
