using UnityEngine;

namespace DTIS
{
    public abstract class EntityBrain : MonoBehaviour {
        protected enum Directions // might move to utils later
        {
            Left = -1,
            Right = 1
        };
        [SerializeField] private float _jumpSpeed = 400f;							// Amount of force added when the entity jumps.
        [SerializeField] private float _walkSpeed = 10f;                            // How fast is the entity, running will mutliply this value!
        [SerializeField] private float _runSpeedMult = 1.75f;                       // entities in the Run state will move at a speed of _walkspeed * _runSpeedMult.
        [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
        private EntityStateMachine _fsm;
        protected EntityStateMachine FSM { get { return _fsm; } set { _fsm = value; }}
        protected virtual void Awake()
        {
            FSM = gameObject.AddComponent<EntityStateMachine>(); // adds the entity FSM component and sets it via a property
            FSM.setControllerSpeeds(_jumpSpeed,_walkSpeed,_runSpeedMult ,_movementSmoothing);
        }
        protected virtual void Update()
        {
            Logic();
        }
        protected abstract void Logic();
    }
}