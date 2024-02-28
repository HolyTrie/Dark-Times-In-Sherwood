using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    /* sources
        1. https://www.youtube.com/watch?v=h08yjfxO6FA&ab_channel=1zed1Games
    */
    public class PlayerControls : MonoBehaviour
    {
        public static PlayerControls Instance {get; private set;}
        private PlayerActionMap _am;
        public PlayerActionMap ActionMap { get { return _am; } }
        private float _walking = 0f;
        private bool _runIsPressed = false;
        public float WalkingDirection { get { return _walking; } private set { _walking = value; } }
        public bool Running {get{return _runIsPressed;}}

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
            Debug.Log(Running);
        }

        private void OnEnable()
        {
            _am ??= new PlayerActionMap();
            SetRun(true);
            _am.Enable();
        }

        private void OnDisable()
        {
            SetRun(false);
            _am.Disable();
        }

        private void SetRun(bool value)
        {
            if(value)
            {
                //_am.All.Run.started += Started;
                _am.All.Run.canceled += Canceled;
                _am.All.Run.performed += Performed;
            }
            else
            {
                //_am.All.Run.started -= Started;
                _am.All.Run.canceled -= Canceled;
                _am.All.Run.performed -= Performed;
            }
            void Started(InputAction.CallbackContext ctx) { _runIsPressed = ctx.started; }
            void Canceled(InputAction.CallbackContext ctx) { _runIsPressed = !ctx.canceled; }
            void Performed(InputAction.CallbackContext ctx) {_runIsPressed = ctx.performed; }
        }
    }


}