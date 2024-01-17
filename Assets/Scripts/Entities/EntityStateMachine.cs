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
        private Animator _animator;
        private EntityState _state;
        private EntityState _subState;
        //private EntityState _prevState; // TODO: this should be a stack of states instead. (with curr being the top) - better solution even though the stack is capped at height 2
        protected EntityState State // Property with simple getter and a setter that handles state switch by calling exit and enter appropriately.
        {
            get { return _state; } 
            set 
            { 
                _state.Exit();
                _state = value;
                _state.Enter();
            }
        } 
        protected EntityState SubState // Property with simple getter and a setter that handles sub-state switch by calling exit and enter appropriately.
        {
            get { return _subState; } 
            set 
            { 
                _subState.Exit();
                _subState = value;
                _subState.Enter();
            }
        } 

        protected virtual void Awake(){
            _controller = GetComponent<EntityController>();
            _animator = GetComponent<Animator>();
        }
        protected virtual void Update(){

        }

        protected virtual void FixedUpdate(){

        }
  
    }
}