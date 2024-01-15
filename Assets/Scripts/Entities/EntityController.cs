using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS
{
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] private float m_JumpSpeed = 400f;							// Amount of force added when the entity jumps.
        [SerializeField] private float m_WalkSpeed = 50f;                               // How fast is the entity.
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
        [SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
        [HideInInspector] private bool facingRight = true;                                            // A boolean marking the entity's orientation.
        [HideInInspector] public Rigidbody2D m_Rigidbody2D;                         // for manipulating an entity's physics by an IEntityMovement
        [HideInInspector] private Vector3 m_Velocity = Vector3.zero;
        [SerializeField] private IEntityBehaviour behaviour;
        [SerializeField] private IEntityMovement movement;
        [SerializeField] private EntityStateMachine fsm;
        void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //behaviour.Update(movement,fsm);
        }

        void FixedUpdate() {
            behaviour.FixedUpdate(movement,fsm,this);
        }

        public float velY(){ return m_Rigidbody2D.velocity.y; }
        public float velX(){ return m_Rigidbody2D.velocity.x; }

        public void applySmoothDamp(Vector3 targetVelocity)
        {
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }

        public float WalkSpeed() { return m_WalkSpeed; }
        public float JumpSpeed() { return m_JumpSpeed; }

    }
}


