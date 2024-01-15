using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the entity jumps.
    [SerializeField] private float m_Speed = 50f;                               // How fast is the entity.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the entity is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    // Start is called before the first frame update
    void Start()
    {
        // set 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
