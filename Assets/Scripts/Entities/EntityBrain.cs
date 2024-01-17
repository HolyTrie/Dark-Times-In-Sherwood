using UnityEngine;

namespace DTIS
{
    public abstract class EntityBrain : MonoBehaviour {
        enum Directions // might move to utils later
        {
            Left = -1,
            Right = 1
        };
        private Directions _direction;
        protected float Direction{ 
            get 
            { 
                return (float)_direction; 
            }
            set
            { 
                _direction = value >=0 ? Directions.Right : Directions.Left;
            }
         }
        private EntityStateMachine _fsm;
        protected EntityStateMachine FSM { get { return _fsm; } set { _fsm = value; }}
        protected virtual void Awake()
        {
            FSM = gameObject.AddComponent<EntityStateMachine>(); // adds the entity FSM component and sets it via a property
            Direction = (float)Directions.Right;
        }
        protected abstract void Update();
        protected abstract void FixedUpdate();
    }
}