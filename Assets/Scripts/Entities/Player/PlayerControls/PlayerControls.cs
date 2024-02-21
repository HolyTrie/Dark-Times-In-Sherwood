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
        private float _horizontalMove = 0f;
        private bool _isRunning = false;
        public float HorizontalMove { get { return _horizontalMove; } set { _horizontalMove = value; } }

        private void Awake()
        {
            _am = new PlayerActionMap();
        }
        void Update() 
        {
            //if(ActionMap.All.Run.WasPressedThisFrame()) {_isRunning = true;}
            //if(ActionMap.All.Run.WasReleasedThisFrame()) {_isRunning = false;}
        }
        void FixedUpdate()
        {
            /*
            if(_isRunning)
                HorizontalMove = ActionMap.All.Run.ReadValue<int>();
            else*/
            HorizontalMove = ActionMap.All.Walk.ReadValue<float>();
            
        }

        void OnEnable()
        {
            ActionMap.Enable();
        }

        void OnDisable()
        {
            ActionMap.Disable();
        }
    }


}