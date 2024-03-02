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
        public static PlayerControls Instance { get; private set; }
        private PlayerActionMap _am;
        public PlayerActionMap ActionMap { get { return _am; } }
        private float _walking = 0f;
        private bool _runIsPressed = false;

        private bool _jumpIsPressed = false;
        private bool _downIsPressed = false;
        public float WalkingDirection { get { return _walking; } private set { _walking = value; } }
        public bool RunIsPressed { get { return _runIsPressed; } }
        public bool JumpIsPressed { get { return _jumpIsPressed; } }
        public bool DownIsPressed { get { return _downIsPressed; } }
        public bool DownJumpIsPressed { get { return _downIsPressed && _jumpIsPressed; } }

        private void Awake()
        {
            if (Instance == null)
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
            SetRun(true);
            SetJump(true);
            SetDown(true);
            _am.Enable();
        }

        private void OnDisable()
        {
            SetRun(false);
            SetJump(false);
            SetDown(false);
            _am.Disable();
        }

        private void SetRun(bool value)
        {
            if (value)
            {
                _am.All.Run.started += Started;
                _am.All.Run.canceled += Canceled;
                _am.All.Run.performed += Performed;
            }
            else
            {
                _am.All.Run.started -= Started;
                _am.All.Run.canceled -= Canceled;
                _am.All.Run.performed -= Performed;
            }
            void Started(InputAction.CallbackContext ctx) { _runIsPressed = ctx.started; }
            void Canceled(InputAction.CallbackContext ctx) { _runIsPressed = !ctx.canceled; }
            void Performed(InputAction.CallbackContext ctx) { _runIsPressed = ctx.performed; }
        }
        private void SetJump(bool value)
        {
            if (value)
            {
                _am.All.Jump.started += Started;
                _am.All.Jump.canceled += Canceled;
                _am.All.Jump.performed += Performed;
            }
            else
            {
                _am.All.Jump.started -= Started;
                _am.All.Jump.canceled -= Canceled;
                _am.All.Jump.performed -= Performed;
            }
            void Started(InputAction.CallbackContext ctx) { _jumpIsPressed = ctx.started; }
            void Canceled(InputAction.CallbackContext ctx) { _jumpIsPressed = !ctx.canceled; }
            void Performed(InputAction.CallbackContext ctx) { _jumpIsPressed = ctx.performed; }
        }
        private void SetDown(bool value)
        {
            if (value)
            {
                _am.All.Down.started += Started;
                _am.All.Down.canceled += Canceled;
                _am.All.Down.performed += Performed;
            }
            else
            {
                _am.All.Down.started -= Started;
                _am.All.Down.canceled -= Canceled;
                _am.All.Down.performed -= Performed;
            }
            void Started(InputAction.CallbackContext ctx) { _downIsPressed = ctx.started; }
            void Canceled(InputAction.CallbackContext ctx) { _downIsPressed = !ctx.canceled; }
            void Performed(InputAction.CallbackContext ctx) { _downIsPressed = ctx.performed; }
        }
    }


}