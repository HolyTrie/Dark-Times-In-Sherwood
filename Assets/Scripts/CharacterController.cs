using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 100f;                          // Amount of force added when the player jumps.
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;           // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

    [SerializeField]
    InputAction MoveLeft = new(type: InputActionType.Button);

    [SerializeField]
    InputAction MoveRight = new(type: InputActionType.Button);

    [SerializeField]
    InputAction Jump = new(type: InputActionType.Button);

    [SerializeField]
    float Speed;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;

    Transform player;

    Animator player_anim;

    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
        player_anim = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                // Debug.Log(m_Grounded);
            }

        }
    }

    public void Move(float speed, bool crouch, bool can_jump)
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
                // Reduce the speed by the crouchSpeed multiplier
                speed *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;
            }


            MovementDirecton();

        }

        PlayerJump();

    }

    void OnEnable()
    {
        MoveRight.Enable();
        MoveLeft.Enable();
        Jump.Enable();
    }

    void OnDisable()
    {
        MoveRight.Disable();
        MoveLeft.Disable();
        Jump.Disable();
    }

    //tells the player to jump
    void PlayerJump()
    {
        // Debug.Log("Pressed Jump!");
        if (Jump.IsPressed())
        {
            Debug.Log(m_Grounded);
            // If the player should jump...
            if (m_Grounded)
            {
                Debug.Log("Jumping");
                player_anim.SetBool("Jumping", true);
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                player.position += new Vector3(0, m_JumpForce * Time.deltaTime, 0);
            }

        }
        else
        {
            player_anim.SetBool("Jumping", false);
        }

    }

    //tells the player where to move
    void MovementDirecton()
    {
        if (MoveLeft.IsPressed())
        {
            Debug.Log("Player moving left");
            player_anim.SetBool("Walking", true);
            player.position -= new Vector3(Speed * Time.deltaTime, 0, 0);
            player.localScale = new Vector3(1, 1, 1);

        }
        else if (MoveRight.IsPressed())
        {
            Debug.Log("Player moving Right");
            player_anim.SetBool("Walking", true);
            player.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            player.localScale = new Vector3(-1, 1, 1);
        }
        else if (!MoveRight.IsPressed() && !MoveLeft.IsPressed())
        {
            player_anim.SetBool("Walking", false);
        }
    }

}
