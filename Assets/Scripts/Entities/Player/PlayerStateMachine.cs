using System;
using UnityEngine;

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
        enum Directions // might move to utils later
        {
            Left = -1,
            Right = 1
        };
        private Directions _direction;
        public float Direction{ 
            get 
            { 
                return (float)_direction; 
            }
            set
            { 
                _direction = value >=0 ? Directions.Right : Directions.Left;
            }
         }
        private PlayerController _controller;
        private PlayerControls _controls;
        public PlayerControls Controls{get{return _controls;}}
        private bool _grounded = true;
        public bool Grounded{get{return _grounded;} set{_grounded = value;}}
        private PlayerState _state;
        private PlayerState _subState;
        //private PlayerState _prevState; // TODO: this should be a stack of states instead. (with curr being the top) - better solution.
        public PlayerState State // Property with simple getter and a setter that handles state switch by calling exit and enter appropriately.
        {
            get { return _state; } 
            set 
            { 
                _state?.Exit();
                _state = value;
                _state.Enter(_controller,this); // delegates state and sub-state switch to state implementation!
            }
        } 
        public PlayerState SubState // Property with simple getter and a setter that handles sub-state switch by calling exit and enter appropriately.
        {
            get { return _subState; } 
            set 
            { 
                _subState?.Exit();
                _subState = value;
                _subState.Enter(_controller,this); // delegates state and sub-state switch to state implementation!
            }
        }

        protected void Awake()
        {
            _controller = gameObject.GetComponent<PlayerController>(); 
            _controls = GetComponent<PlayerControls>();
            SetState(ESP.States.Grounded,ESP.States.Idle);
            Direction = (float)Directions.Right;
        }

        protected void Start()
        {
            Controls.ActionMap.All.GoGhost.performed += _ => _controller.Ghost();
        }
        public virtual void SetState(ESP.States state, ESP.States subState)
        {
            State = ESP.Build(state);
            SubState = ESP.Build(subState);
        }
        protected virtual void Update(){
            _state?.Update();
            _subState?.Update();
        }

        protected virtual void FixedUpdate(){
           _state?.FixedUpdate(); 
           _subState?.FixedUpdate();
        }

        // prints the state of the player above his head//
        private void OnGUI()
        {
            var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float x,y,width,height;
            x = position.x - 50;
            y = Screen.height - position.y - 215;
            int textSize = 25;
            int smallerTextSize = textSize - 5;
            width = 200f;
            height = 100f;
            //Debug.Log(String.Format("x = {0}, y = {1}, width = {2}, height = {3}",x,y,width,height));
            Rect MainState = new Rect(x, y, width, height);
            Rect SubState = new Rect(x+30f, y+25f, width, height);
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