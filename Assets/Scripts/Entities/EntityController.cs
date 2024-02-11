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
        [SerializeField] private float _jumpSpeed, _walkSpeed, _runSpeedMult , _movementSmoothing ;
        public float JumpSpeed{get { return _jumpSpeed;} set{_jumpSpeed = value;}}
        public float WalkSpeed{get { return _walkSpeed;} set{_walkSpeed = value;}}
        public float RunSpeedMult{get { return _runSpeedMult;} set{_runSpeedMult = value;}}
        public float MoveSmoothSpeed{get { return _movementSmoothing;} set{_movementSmoothing = value;}}
        [SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
        [HideInInspector] private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        [HideInInspector] private Rigidbody2D _rb2D;                         // for manipulating an entity's physics by an IEntityMovement
        [HideInInspector] private Vector3 _Velocity = Vector3.zero;                // Entitys current velocity as a 3D vector. 
        private Animator _animator;
        public Animator Animator{get{return _animator;}}
        void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
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
            Vector3 targetVelocity = new Vector2(_walkSpeed * speedMultiplier, _rb2D.velocity.y); // Move the character by finding the target velocity
            _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _Velocity, _movementSmoothing); // And then smoothing it out and applying it to the character
        }
        public virtual void Move(Vector2 velocityMult)
        {
            MoveWithSmoothDamp(velocityMult);
        }
        protected virtual void Flip()
        {
            bool movingRight = _rb2D.velocity.x > 0;
            bool movingLeft = _rb2D.velocity.x < 0;
            if (_facingRight && movingLeft)
            {
                _facingRight = !_facingRight;
                transform.GetComponent<SpriteRenderer>().flipX = true; // flip to face Left
            }
            if (!_facingRight && movingRight)
            {
                _facingRight = !_facingRight;
                transform.GetComponent<SpriteRenderer>().flipX = false; // flip to face Right
            }
        }
        //private void Walk();
        //private void Jump();
        //private void Run();


    }
}


