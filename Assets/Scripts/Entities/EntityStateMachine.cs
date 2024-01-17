using System;
using Unity.VisualScripting;
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
    public class EntityStateMachine : MonoBehaviour 
    {
        private EntityController _controller;
        private EntityState _state;
        private EntityState _subState;
        //private EntityState _prevState; // TODO: this should be a stack of states instead. (with curr being the top) - better solution even though the stack is capped at height 2
        public EntityState State // Property with simple getter and a setter that handles state switch by calling exit and enter appropriately.
        {
            get { return _state; } 
            set 
            { 
                _state.Exit(_controller);
                _state = value;
                _state.Enter(_controller);
            }
        } 
        public EntityState SubState // Property with simple getter and a setter that handles sub-state switch by calling exit and enter appropriately.
        {
            get { return _subState; } 
            set 
            { 
                _subState.Exit(_controller);
                _subState = value;
                _subState.Enter(_controller);
            }
        } 

        protected void Awake(){
            /*
            try
            {
                _controller = GetComponent<EntityController>(); // for applying derived controllers
                //Debug.Log("FSM found attached controller");
            }
            catch
            {
                Debug.Log("FSM did not find attached controller");
                _controller = gameObject.AddComponent<EntityController>(); // adds the base controller and sets _controller 
                Debug.Log("FSM controller was set to " + _controller);
            }
            */
            _controller = gameObject.AddComponent<EntityController>();
            SetInitialState(ESP.States.Grounded,ESP.States.Idle);
        }
        public virtual void SetState(ESP.States state, ESP.States subState)
        {
            State = ESP.Build(state);
            SubState = ESP.Build(subState);
        }
        protected virtual void SetInitialState(ESP.States state, ESP.States subState)
        {
            _state = ESP.Build(state);
            _subState = ESP.Build(subState);
        }
        protected virtual void Update(){
            _state?.Update(this,_controller); // delegates state and sub-state switch to state implementation!
            _subState?.Update(this,_controller); // delegates state and sub-state switch to state implementation!
        }

        protected virtual void FixedUpdate(){
           _state?.FixedUpdate(this,_controller); // delegates state and sub-state switch to state implementation!
           _subState?.FixedUpdate(this,_controller); // delegates state and sub-state switch to state implementation!
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
            string content = _state != null ? _state.Name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10f, 60f, 200f, 100f));
            content = _subState != null ? _subState.Name : "(no current state)";
            GUILayout.Label($"<color='black'><size=30>{content}</size></color>");
            GUILayout.EndArea();
        }
  
    }
}