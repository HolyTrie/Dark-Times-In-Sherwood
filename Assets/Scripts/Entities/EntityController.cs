using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        private float _jumpSpeed, _walkSpeed, _runSpeedMult , _movementSmoothing ;
        // public float JumpSpeed{get { return _jumpSpeed;}}
        // public float WalkSpeed{get { return _walkSpeed;}}
        public float RunSpeedMult{get { return _runSpeedMult;}}
        // public float MoveSmoothSpeed{get { return _movementSmoothing;}}
        public virtual void setControllerSpeeds(float jumpSpeed, float walkSpeed, float runSpeedMult, float movementSmoothing)
        {
            _jumpSpeed = jumpSpeed;
            _walkSpeed = walkSpeed;
            _runSpeedMult = runSpeedMult;
            _movementSmoothing = movementSmoothing;
        }
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
        public void MoveWithSmoothDamp(Vector2 velocityMult)
        {
            float speedMultiplier = velocityMult.x;
            Vector3 targetVelocity = new Vector2(_walkSpeed * speedMultiplier, _rigidbody2D.velocity.y); // Move the character by finding the target velocity
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _Velocity, _movementSmoothing); // And then smoothing it out and applying it to the character
        }
        public virtual void Move(Vector2 velocityMult)
        {
            MoveWithSmoothDamp(velocityMult);
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


