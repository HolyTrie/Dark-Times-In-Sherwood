using UnityEngine;

namespace DTIS
{
    public abstract class EntityBehaviour : MonoBehaviour {
        private EntityStateMachine _fsm;
        protected FSM { get { return _fsm; } set { _fsm = value; } } // not sure setter is neede but there it is
        virtual void Awake(){
            _fsm = GetComponent<EntityStateMachine>();
        }
        virtual void Update();
        virtual void FixedUpdate();
    }
}