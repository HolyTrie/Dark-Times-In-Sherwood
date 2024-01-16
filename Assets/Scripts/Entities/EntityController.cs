using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS
{
    /*
        Responsible for all 'basic' aspects of entity management in unity like position and physics, acting as a template class for other classes to override and specialize.
    */
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] private float m_JumpSpeed = 400f;							// Amount of force added when the entity jumps.
        [SerializeField] private float m_WalkSpeed = 50f;                           // How fast is the entity, running will mutliply this value!
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
        [SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
        [HideInInspector] private bool facingRight = true;                          // A boolean marking the entity's orientation.
        [HideInInspector] public Rigidbody2D m_Rigidbody2D;                         // for manipulating an entity's physics by an IEntityMovement
        [HideInInspector] private Vector3 m_Velocity = Vector3.zero;
        void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate() {

        }

        public void MoveWithSmoothDamp(float speedMultiplier)
        {
            Vector3 targetVelocity = new Vector2(m_WalkSpeed * speedMultiplier, m_Rigidbody2D.velocity.y); // Move the character by finding the target velocity
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing); // And then smoothing it out and applying it to the character
        }

        public void Move(Vector2 speed, bool jump = false)
        {
            //TODO
        }
        //private void Walk();
        //private void Jump();
        //private void Run();


    }
}


