using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS
{
    /*
        Responsible for all 'basic' aspects of entity management in unity like position and physics, acting as a template class for other classes to override and specialize.
        Important reference:
        - https://stackoverflow.com/questions/12662072/what-is-protected-virtual-new
    */
    public class EntityController : MonoBehaviour
    {
        [SerializeField] private float _jumpSpeed = 400f;							// Amount of force added when the entity jumps.
        [SerializeField] private float _walkSpeed = 10f;                            // How fast is the entity, running will mutliply this value!
        [SerializeField] private float _runSpeedMult = 1.75f;                       // entities in the Run state will move at a speed of _walkspeed * _runSpeedMult.
        [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
        [SerializeField] private LayerMask _whatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Transform _groundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
        [HideInInspector] private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        [HideInInspector] private Rigidbody2D _rigidbody2D;                         // for manipulating an entity's physics by an IEntityMovement
        [HideInInspector] private Vector3 _Velocity = Vector3.zero;                // Entitys current velocity as a 3D vector. 
        private Animator _animator;
        public Animator Animator{get{return _animator;}}

        private Transform _transform;
        void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _transform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        void FixedUpdate() {
            Flip();
        }
        public void MoveWithSmoothDamp(float speedMultiplier)
        {
            Vector3 targetVelocity = new Vector2(0.1f * _walkSpeed * speedMultiplier, _rigidbody2D.velocity.y); // Move the character by finding the target velocity
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _Velocity, _movementSmoothing); // And then smoothing it out and applying it to the character
        }

        public virtual void Move(float speedMultiplier, bool jump = false)
        {
            MoveWithSmoothDamp(speedMultiplier);
        }
        public virtual void Move(Vector2 speed, bool jump = false)
        {
            //TODO?
        }
        protected virtual void Flip()
        {
            if((_facingRight && _rigidbody2D.velocity.x < 0) || (!_facingRight && _rigidbody2D.velocity.x > 0))
            {
                _facingRight = !_facingRight;
                _transform.localScale = Vector3.Scale(_transform.localScale, new Vector3(-1,1,1));
            }
        }
        //private void Walk();
        //private void Jump();
        //private void Run();


    }
}


