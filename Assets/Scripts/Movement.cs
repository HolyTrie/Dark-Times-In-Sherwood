using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float Speed;

    public CharacterController controller;
	bool jump = false;
	bool crouch = false;

   
    void Start()
    {
        Debug.Log("Start Moving");
    }
	void FixedUpdate ()
	{
		// Move our character
		controller.Move(Speed * Time.fixedDeltaTime, crouch, jump);
		jump = false;

    }

   
}
