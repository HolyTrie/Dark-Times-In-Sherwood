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
        public float HorizontalMove { get { return _horizontalMove; } set { _horizontalMove = value; } }

        private void Awake()
        {
            _am = new PlayerActionMap();
        }
        void FixedUpdate()
        {
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