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
        public PlayerActionMap ActionMap { get { return _am; } }
        public float WalkingDirection { get { return _horizontalDirection; } private set { _horizontalDirection = value; } }
        public float VerticalInput { get { return _verticalDirection; } private set { _verticalDirection = value; } }
        public bool RunIsPressed { get { return _runIsPressed; } }
        public bool JumpIsPressed { get { return _jumpIsPressed; } }
        public bool DownIsPressed { get { return VerticalInput == -1f; } }
        public bool UpIsPressed { get { return VerticalInput == 1f; } }
        public bool DownJumpIsPressed { get { return DownIsPressed && JumpIsPressed; } }

        public bool ReadHorizontalInput { get { return _readHorizontalInput; } set { _readHorizontalInput = value; } }

        private PlayerActionMap _am;
        private GameObject _pauseMenu;
        private float _horizontalDirection = 0f;
        private float _verticalDirection = 0f;
        private bool _runIsPressed = false;
        private bool _jumpIsPressed = false;
        private bool _readHorizontalInput = true;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            _am = new PlayerActionMap();

            _pauseMenu = GameObject.Find("PauseMenu");
        }

        private void Start()
        {
            SetPauseMenuActive(false);
        }

        private void SetPauseMenuActive(bool val)
        {
            if(_pauseMenu != null)
                _pauseMenu.SetActive(val);
        }

        private void Update()
        {
            if (ActionMap.All.PauseMenu.WasPressedThisFrame())
            {
                GameManager.PauseGame();
                SetPauseMenuActive(true);
            }
        }

        private void FixedUpdate()
        {
            WalkingDirection = ActionMap.All.Horizontal.ReadValue<float>();
            if(!_readHorizontalInput)
                WalkingDirection = 0f;
            VerticalInput = ActionMap.All.Vertical.ReadValue<float>();
        }

        private void OnEnable()
        {
            _am ??= new PlayerActionMap();
            SetRun(true);
            SetJump(true);
            _am.Enable();
        }

        private void OnDisable()
        {
            SetRun(false);
            SetJump(false);
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
    }


}