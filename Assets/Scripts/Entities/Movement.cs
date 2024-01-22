using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    InputAction MoveLeft = new(type: InputActionType.Button);

    [SerializeField]
    InputAction MoveRight = new(type: InputActionType.Button);

    [SerializeField]
    float Speed;

    Transform player;

    Animator player_anim;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Moving");
        player = GetComponent<Transform>();
        player_anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        MovementDirecton();
    }


    void OnEnable()
    {
        MoveRight.Enable();
        MoveLeft.Enable();
    }

    void OnDisable()
    {
        MoveRight.Disable();
        MoveLeft.Disable();
    }

    //tells the player where to move
    void MovementDirecton()
    {
        if(MoveLeft.IsPressed())
        {
            Debug.Log("Player moving left");
            player_anim.SetFloat("Walking", Input.GetAxis("MoveLeft") );
            player.position -= new Vector3 (Speed*Time.deltaTime,0,0);
            player.localScale = new Vector3 (1,1,1);
            
        }
        else if(MoveRight.IsPressed())
        {
            Debug.Log("Player moving Right");
            player_anim.SetFloat("Walking", Input.GetAxis("MoveRight") );
            player.position += new Vector3 (Speed*Time.deltaTime,0,0);
            player.localScale = new Vector3 (-1,1,1);
            
        }
        else if(!MoveRight.IsPressed() && !MoveLeft.IsPressed())
        {
            player_anim.SetBool("IsMoving",false);
        }
    }
}
