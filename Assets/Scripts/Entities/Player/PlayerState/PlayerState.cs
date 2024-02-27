using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DTIS
{
    /// <summary>
    /// Every state has 3 key responsibilities which are unique to it's implementation.
    /// 1. Setting the correct graphic/animation in the Animator component via a property of the controller.
    /// 2. Checking conditions to switch to legal states and sub-states, and making the switch via properties in the FSM.
    /// 3. update the entity behaviour via the controller in every update and fixed update, for example walk and run manipulate the velocity of an entity.
    /// </summary>
    public abstract class PlayerState
    {
        private readonly string _name;
        public virtual string Name { get { return _name; } }
        private ESP.States _state;
        public ESP.States Type{get{return _state;}}
        private readonly bool _hasAnimation;
        public virtual bool HasAnimation { get { return _hasAnimation; } }

        private PlayerController _controller;
        public virtual PlayerController Controller { get { return _controller; } }

        private PlayerStateMachine _fsm;
        public virtual PlayerStateMachine FSM { get { return _fsm; } }
        public virtual PlayerControls Controls {get {return _fsm.Controls;}}
        public virtual PlayerActionMap.AllActions ActionMap { get { return FSM.Controls.ActionMap.All; } }
        // TODO: ^ this ^ is currently hard coupled to return any 'auto generated' action map named 'All' <-- fix if time.
        protected virtual void SetStates(ESP.States State, ESP.States SubState) // for changing both at once
        {
            FSM.SetState(State, SubState);
        }
        protected virtual void SetState(ESP.States State) // for changing only state
        {
            FSM.State = ESP.Build(State);
        }
        protected virtual PlayerState State { get { return FSM.State; } set { FSM.State = value; } }
        protected virtual void SetSubState(ESP.States SubState) // for changing only substate
        {
            FSM.SubState = ESP.Build(SubState);
        }
        public PlayerState(ESP.States state,string name, bool hasAnimation = true)
        {
            _state = state;
            _name = name;
            _hasAnimation = hasAnimation;
        }
        public virtual void Enter(PlayerController controller, PlayerStateMachine fsm)
        {
            _controller = controller;
            _fsm = fsm;
        }
        public virtual void Exit(ESP.States State, ESP.States SubState)
        {
            Controller.Animator.StopPlayback();
            /* 
            This is saved here for potential future improvements, triggers could be used with this 
            fsm but pose more problems with how we flip character - more research later!
            if(HasAnimation)
            {
                try
                {
                    Controller.Animator.ResetTrigger(Name); //using triggers will activate transitions as well!
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
            }
            */
        }
        public void Update()
        {
            TryStateSwitch();  // this should check input from player and switch states appropriately
        }
        public void FixedUpdate()
        {
            PhysicsCalculation(); // must be implemented but can remain empty if nothing is to be done!
        }
        protected abstract void TryStateSwitch();
        protected abstract void PhysicsCalculation();
        
        protected private virtual void SetAnimations()
        {
            if (HasAnimation)
            {
                try
                {
                    Controller.Animator.Play(Name);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

    }
}
