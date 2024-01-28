using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DTIS
{
    public class PlayerControls : MonoBehaviour 
    {
        private PlayerActionMap _am;
        public PlayerActionMap ActionMap{get{return _am;}}
        public InputAction moveHorizontal = new(type: InputActionType.Button);

        //public InputAction Attack2 = new(type: InputActionType.Button);
        private float _horizontalMove = 0f;
        public float HorizontalMove{ get {return _horizontalMove;} set {_horizontalMove = value;}}

        private void Awake()
        {
            _am = new PlayerActionMap();
        }
        private void Start() {
            LogControls();
        }

        private void LogControls()
        {
            //TODO
        }
        void Update() {
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