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
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool _airControl = true;
        [SerializeField] private float _jumpForce = 15f;
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _runSpeedMult = 1.75f;
        public float RunSpeedMult{get { return _runSpeedMult;}}
        [SerializeField] private float _movementSmoothing = 0.35f;
        [SerializeField] private LayerMask _whatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider to be disabled on the 'crouch' player action.
        [SerializeField] private Transform _groundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
        private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        private Rigidbody2D _rb2D;                         // for manipulating an entity's physics by an IEntityMovement
        public Vector3 Velocity{get{return _rb2D.velocity;}}
        private Vector3 _Velocity = Vector3.zero;                // Entitys current velocity as a 3D vector. 
        private Animator _animator;
        public Animator Animator{get{return _animator;}}

        private Transform _transform;
        void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _transform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Flip();
        }
        void FixedUpdate() {
            
        }
        
        protected virtual void Flip()
        {
            if((_facingRight && _rb2D.velocity.x < 0) || (!_facingRight && _rb2D.velocity.x > 0))
            {
                _facingRight = !_facingRight;
                _transform.localScale = Vector3.Scale(_transform.localScale, new Vector3(-1,1,1));
            }
        }
        public void MoveWithSmoothDamp(Vector2 velocityMult)
        {
            float speedMultiplier = velocityMult.x;
            Vector3 targetVelocity = new Vector2(_walkSpeed * speedMultiplier, _rb2D.velocity.y); // Move the character by finding the target velocity
            _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _Velocity, _movementSmoothing); // And then smoothing it out and applying it to the character
        }
        public virtual void Move(Vector2 velocityMult)
        {
            MoveWithSmoothDamp(velocityMult);
        }

        public virtual void Jump(float forceMult = 1)
        {
            _rb2D.AddForce(new Vector2(0,_jumpForce * forceMult),ForceMode2D.Impulse);
        }


    }
}


