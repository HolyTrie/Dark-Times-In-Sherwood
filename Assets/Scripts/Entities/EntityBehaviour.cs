using UnityEngine;

namespace DTIS
{
    public abstract class EntityBehaviour : MonoBehaviour {
        private EntityStateMachine _fsm;
        protected EntityStateMachine FSM { get { return _fsm; } set { _fsm = value; } } // not sure setter is needed but there it is
        protected virtual void Awake(){
            _fsm = GetComponent<EntityStateMachine>();
        }
        protected abstract void Update();
        protected abstract void FixedUpdate();
    }
}