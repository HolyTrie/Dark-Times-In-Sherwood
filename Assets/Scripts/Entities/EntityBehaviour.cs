using UnityEngine;

namespace DTIS
{
    public abstract class EntityBehaviour : MonoBehaviour {
        public EntityStateMachine m_fsm;
        public EntityController m_controller;
        public void Update()
        {
            return;
        }
        public void FixedUpdate()
        {
            return;
        }
    }
}