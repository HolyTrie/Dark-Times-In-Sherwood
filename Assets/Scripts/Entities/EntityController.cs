using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS
{
    /*
        Responsible for all 'basic' aspects of entity management in unity like position and physics, acting as a template class for other classes to override and specialize.
        Important reference:
        - https://stackoverflow.com/questions/12662072/what-is-protected-virtual-new
    */
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] private float m_JumpSpeed = 400f;							// Amount of force added when the entity jumps.
        [SerializeField] private float m_WalkSpeed = 10f;                           // How fast is the entity, running will mutliply this value!
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
        [SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
        [HideInInspector] private bool m_facingRight = true;                          // A boolean marking the entity's orientation.
        [HideInInspector] public Rigidbody2D m_Rigidbody2D;                         // for manipulating an entity's physics by an IEntityMovement
        [HideInInspector] private Vector3 m_Velocity = Vector3.zero; // TODO: wtf is this

        private Transform m_Transform;
        void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate() {
            Flip();
        }

        public void MoveWithSmoothDamp(float speedMultiplier)
        {
            Vector3 targetVelocity = new Vector2(0.1f * m_WalkSpeed * speedMultiplier, m_Rigidbody2D.velocity.y); // Move the character by finding the target velocity
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing); // And then smoothing it out and applying it to the character
        }

        public virtual void Move(float speedMultiplier, bool jump = false)
        {
            MoveWithSmoothDamp(speedMultiplier);
        }
        public virtual void Move(Vector2 speed, bool jump = false)
        {
            //TODO
        }

        protected virtual void Flip()
        {
            if(m_facingRight && m_Rigidbody2D.velocity.x < 0)
            {
                m_facingRight = false;
                m_Transform.localScale = Vector3.Scale(m_Transform.localScale, new Vector3(-1,1,1));
            }
            if(!m_facingRight && m_Rigidbody2D.velocity.x > 0)
            {
                m_facingRight = true;
                m_Transform.localScale = Vector3.Scale(m_Transform.localScale, new Vector3(-1,1,1));
            }
        }
        //private void Walk();
        //private void Jump();
        //private void Run();


    }
}


