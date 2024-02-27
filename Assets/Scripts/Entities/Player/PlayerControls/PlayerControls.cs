using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    public class PlayerControls : MonoBehaviour
    {
        // public static event Action A;
        public static PlayerControls Instance { get; private set; }
        private PlayerActionMap _am;
        public PlayerActionMap ActionMap { get { return _am; } }
        private float _walking = 0f;
        public float WalkingDirection { get { return _walking; } private set { _walking = value; } }
        public bool Running { get; set; }

        private GameObject PauseMenu;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            _am = new PlayerActionMap();

            PauseMenu = GameObject.Find("PauseMenu");
        }

        private void Update()
        {
            if (ActionMap.All.PauseMenu.WasPressedThisFrame())
            {
                GameManager.PauseGame();
                PauseMenu.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            WalkingDirection = ActionMap.All.Walk.ReadValue<float>();
            Running = ActionMap.All.Run.WasPerformedThisFrame();
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