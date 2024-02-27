using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    public class PlayerControls : MonoBehaviour
    {
        // public static event Action A;
        public static PlayerControls Instance {get; private set;}
        private PlayerActionMap _am;
        public PlayerActionMap ActionMap { get { return _am; } }
        private float _walking = 0f;
        private float _running = 0f;
        public float WalkingDirection { get { return _walking; } private set { _walking = value; } }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            _am = new PlayerActionMap();
        }
        
        private void FixedUpdate()
        {
            WalkingDirection = ActionMap.All.Walk.ReadValue<float>();
        }

        private void OnEnable()
        {
            _am ??= new PlayerActionMap();
            _am.Enable();
        }

        private void OnDisable()
        {
            _am.Disable();
        }
    }


}