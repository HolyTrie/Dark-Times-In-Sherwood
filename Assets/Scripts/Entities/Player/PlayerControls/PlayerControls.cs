using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    public class PlayerControls : MonoBehaviour
    {
        // public static event Action A;

        private PlayerActionMap _am;
        public PlayerActionMap ActionMap { get { return _am; } }
        private float _walking = 0f;
        private float _running = 0f;
        public float RunningDirection {get{return _running;}private set{_running=value;}}
        public float WalkingDirection { get { return _walking; } private set { _walking = value; } }

        private void Awake()
        {
            _am = new PlayerActionMap();
        }
        private void Start()
        {
            //ActionMap.All.Dash.started += _ => Debug.Log("Started interaction");
            //ActionMap.All.Dash.performed += _ => Debug.Log("Performed interaction");
            //ActionMap.All.Dash.canceled += _ => Debug.Log("Cancelled interaction");
        }
        private void Update()
        {
            //
        }
        private void FixedUpdate()
        {
            RunningDirection = ActionMap.All.Run.ReadValue<float>();
            WalkingDirection = ActionMap.All.Walk.ReadValue<float>();
        }

        private void OnEnable()
        {
            ActionMap.Enable();
        }

        private void OnDisable()
        {
            ActionMap.Disable();
        }
    }


}