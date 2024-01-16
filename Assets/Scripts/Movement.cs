using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    InputAction MoveLeft = new(type: InputActionType.Button);

    [SerializeField]
    InputAction MoveRight = new(type: InputActionType.Button);

    [SerializeField]
    InputAction Jump = new(type: InputActionType.Button);

    [SerializeField]
    InputAction Crouch = new(type: InputActionType.Button);

    public CharacterController controller;

    bool move_left = false;
    bool move_right = false;
    bool jump = false;
    bool crouch = false;


    void Start()
    {
        Debug.Log("Start Moving");
    }

    void FixedUpdate()
    {
        MoveCheck();

        // Move our character
        controller.Move(5f, crouch, jump, move_left, move_right);

        //reset
        jump = false;
        crouch = false;
        move_right = false;
        move_left = false;


    }

    void MoveCheck()
    {
        if(MoveLeft.IsPressed())
        {
            move_left = true;
        }
        if(MoveRight.IsPressed())
        {
            move_right = true;
        }
        if(Jump.IsPressed())
        {
            jump = true;
        }
        if(Crouch.WasPressedThisFrame())
        {
            crouch = true;
        }
    }

    void OnEnable()
    {
        MoveRight.Enable();
        MoveLeft.Enable();
        Jump.Enable();
        Crouch.Enable();
    }

    void OnDisable()
    {
        MoveRight.Disable();
        MoveLeft.Disable();
        Jump.Disable();
        Crouch.Disable();
    }



}
