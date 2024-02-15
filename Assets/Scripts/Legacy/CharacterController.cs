using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 50f;                          // Amount of force added when the player jumps.
    // [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D m_Rigidbody2D;

    //privates//
    Vector2 smooth_stop;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    Transform player;
    Animator player_anim;
    float horizontal;

    private void Start()
    {
        // m_Rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        player_anim = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
    }

    public void Move(float crouch_speed, bool crouch, bool jump, bool move_left, bool move_right, bool dash)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                // Debug.Log("Player crouches");
                player_anim.SetBool("Crouch", true);
                // Reduce the speed by the crouchSpeed multiplier
                // Speed *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;
                // crouch = false;
                player_anim.SetBool("Crouch", false);
            }


            //walking//
            if (move_left)
            {
                // Debug.Log("Player moving left");
                Movement();
                FlipPlayer(1);
            }
            else if (move_right)
            {
                // Debug.Log("Player moving Right");
                Movement();
                FlipPlayer(-1);
            }
            else
            {
                // player_anim.speed = 0.5f;
                player_anim.SetBool("Walking", false);
            }

        }

        //jumping//
        if (jump && m_Grounded)
            PlayerJump();
        else
            player_anim.SetBool("Jumping", false);

        if (dash)
            PlayerDash();
        else
            player_anim.SetBool("Dash", false);

    }


    void PlayerDash()
    {
        float DashSpeed = 100f; // tmp //
        player_anim.SetBool("Dash", true);
        m_Rigidbody2D.velocity = new Vector2(Mathf.Lerp(horizontal * DashSpeed, 0.5f, 0), m_Rigidbody2D.velocity.y); //lerp is added to smooth transition movment
    }

    //tells the player to jump
    void PlayerJump()
    {
        // Debug.Log("Jumping");
        player_anim.SetBool("Jumping", true);
        // Add a vertical force to the player.
        m_Grounded = false;
        m_Rigidbody2D.AddForce(new Vector2(0f, Mathf.Lerp(m_JumpForce, 0f, 0f))); //lerp is added to smooth transition movment
    }

    //tells the player to move
    void Movement()
    {
        player_anim.SetBool("Walking", true);
        m_Rigidbody2D.velocity = new Vector2(Mathf.Lerp(horizontal * Speed, 0.5f, 0), m_Rigidbody2D.velocity.y); //lerp is added to smooth transition movment
    }

    //flips the player's side (1 to left, -1 to right)
    void FlipPlayer(int side)
    {
        if (side == 1) // left side
        {
            // player.localScale = new Vector3(player.localScale.x, player.localScale.y, player.localScale.z);
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (side == -1) // right side
        {
            // player.localScale = new Vector3(-1*player.localScale.x, player.localScale.y, player.localScale.z);
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
    }


}
