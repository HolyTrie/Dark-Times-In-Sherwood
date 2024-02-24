using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

namespace DTIS
{
    /*
        TODO: DOCs

        Sources:
        1. https://gameprogrammingpatterns.com/state.html                           <---  important for understanding the pattern and the problem domain
        2. https://www.youtube.com/watch?v=kV06GiJgFhc&ab_channel=iHeartGameDev     <---  complex implementation with some advanced C# features
        3. https://www.youtube.com/watch?v=OtUKsjPWzO8&ab_channel=MinaP%C3%AAcheux  <---  very simple implementation (not hierarchical)
        4. 
    */
    public class PlayerStateMachine : MonoBehaviour
    {

        [Tooltip("Used for activating and deactivating debug state machine mode")]
        [SerializeField] bool _debug = false;

        [Header("Player Scripts")]
        [SerializeField] private PlayerController _controller;
        [SerializeField] private PlayerControls _controls;
        [SerializeField] private PlayerInteractor _interactor;
        public PlayerController Controller { get { return _controller; } }

        private void InitChildScripts()
        {
            // this is a fair solution for now, but when this script becomes too complex
            // we may need to refactor this into a new class / design pattern 
            _controller.FSM = this;
            _interactor.Controller = _controller;
        }
        enum Directions // might move to utils later
        {
            Left = -1,
            Right = 1
        };
        private Directions _direction;
        public float Direction
        {
            get
            {
                return (float)_direction;
            }
            set
            {
                _direction = value >= 0 ? Directions.Right : Directions.Left;
            }
        }
        public PlayerControls Controls { get { return _controls; } }
        private PlayerState _state;
        private PlayerState _subState;
        // Refactor option : Hierarchical state machine instead of state + substate
        public PlayerState State // Property with simple getter and a setter that handles state switch by calling exit and enter appropriately.
        {
            get { return _state; }
            set
            {
                _state?.Exit(_state.Type, _subState.Type);
                _PrevState = _state;
                _state = value;
                _state.Enter(_controller, this); // delegates state and sub-state switch to state implementation!
            }
        }

        public PlayerState SubState // Property with simple getter and a setter that handles sub-state switch by calling exit and enter appropriately.
        {
            get { return _subState; }
            set
            {
                _subState?.Exit(_state.Type, _subState.Type);
                _PrevSubState = _subState;
                _subState = value;
                _subState.Enter(_controller, this); // delegates state and sub-state switch to state implementation!
            }
        }

        //ref to prev states to know how to route the animations,etc..//
        private PlayerState _PrevState;
        public PlayerState PrevState { get { return _PrevState; } }
        private PlayerState _PrevSubState;
        public PlayerState PrevSubState { get { return _PrevSubState; } }


        public bool Grounded { get { return _controller.IsGrounded; } }

        protected void Awake()
        {
            if (_controls == null)
                _controls = GetComponent<PlayerControls>();
            SetState(ESP.States.Grounded, ESP.States.Idle);
            Direction = (float)Directions.Right;
            InitChildScripts();
            GameManager.SetFSM(this);
        }
        protected void Start()
        {
            InitControls();
        }

        protected void InitControls()
        {
            Controls.ActionMap.All.GoGhost.performed += _ => _controller.Ghost(); // TODO: set this differently!
            Controls.ActionMap.All.Interaction.performed += _ => _interactor.Interact();
            Controls.ActionMap.All.Dash.performed += _ => _controller.Dash();
        }
        public virtual void SetState(ESP.States state, ESP.States subState)
        {
            State = ESP.Build(state);
            SubState = ESP.Build(subState);
        }
        protected virtual void Update()
        {
            _state?.Update();
            _subState?.Update();
        }

        protected virtual void FixedUpdate()
        {
            _state?.FixedUpdate();
            _subState?.FixedUpdate();
        }

        // prints the state of the player above his head//
        private void OnGUI()
        {
            if (_debug)
            {
                var position = Camera.main.WorldToScreenPoint(_controller.gameObject.transform.position);
                float x, y, width, height;
                x = position.x - 50;
                y = Screen.height - position.y - 215;
                int textSize = 25;
                int smallerTextSize = textSize - 5;
                width = 200f;
                height = 100f;
                //Debug.Log(String.Format("x = {0}, y = {1}, width = {2}, height = {3}",x,y,width,height));
                Rect MainState = new Rect(x, y, width, height);
                Rect SubState = new Rect(x + 30f, y + 25f, width, height);
                GUILayout.BeginArea(MainState);
                string content = _state != null ? _state.Name : "(no current state)";
                GUILayout.Label($"<color='orange'><size={textSize}>{content}</size></color>");
                GUILayout.EndArea();

                GUILayout.BeginArea(SubState);
                content = _subState != null ? _subState.Name : "(no current state)";
                GUILayout.Label($"<color='red'><size={smallerTextSize}>{content}</size></color>");
                GUILayout.EndArea();
            }
        }
    }
}