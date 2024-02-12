using System.Collections;
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
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeedMult;
        public float RunSpeedMult { get { return _runSpeedMult; } }
        [SerializeField] private float _movementSmoothing;
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider to be disabled on the 'crouch' player action.
        [SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
        [SerializeField] private float ShootDelaySeconds;
        [SerializeField] private float ShootReloadSeconds;
        private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        public bool FacingRight { get { return _facingRight; } private set { _facingRight = value; } }
        private Rigidbody2D _rb2D;                         // for manipulating an entity's physics by an IEntityMovement
        public Vector3 Velocity { get { return _rb2D.velocity; } }
        public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
        private Vector3 _Velocity = Vector3.zero;                // Entitys current velocity as a 3D vector. 
        private Animator _animator;
        public Animator Animator { get { return _animator; } }
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private bool isShooting = false;
        private Camera _mainCamera;
        private Renderer _renderer;
        private PlayerGhostBehaviour _gb;
        private GroundCheck _gc;
        public bool IsGrounded { get { return _gc.Grounded(); } }
        private PlayerStateMachine _fsm;
        public PlayerStateMachine FSM { get { return _fsm; } internal set { _fsm = value; } }

        void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _gc = GetComponentInChildren<GroundCheck>();
            _renderer = GetComponent<Renderer>();
            _gb = new PlayerGhostBehaviour(_renderer);
        }

        // Update is called once per frame
        void Update()
        {
            _gb.TrySetGhostStatus();
            Flip();
        }
        void FixedUpdate()
        {
            return;
        }

        /*Flips the chacater according to his velocity*/
        protected virtual void Flip(bool overrideMovement = false)
        {
            if (_rb2D.velocity.x == 0) // if idle
            {
                Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                bool isMouseRightToPlayer = mouseWorldPosition.x > transform.position.x;
                bool isMouseLeftToPlayer = mouseWorldPosition.x < transform.position.x;
                if (FacingRight && isMouseLeftToPlayer)
                {
                    FacingRight = !FacingRight;
                    transform.GetComponent<SpriteRenderer>().flipX = true; // flip to face Left
                }
                if (!FacingRight && isMouseRightToPlayer)
                {
                    FacingRight = !FacingRight;
                    transform.GetComponent<SpriteRenderer>().flipX = false; // flip to face Right
                }
            }
            else
            {
                bool movingRight = _rb2D.velocity.x > 0;
                bool movingLeft = _rb2D.velocity.x < 0;
                if (FacingRight && movingLeft)
                {
                    FacingRight = !FacingRight;
                    transform.GetComponent<SpriteRenderer>().flipX = true; // flip to face Left
                }
                if (!FacingRight && movingRight)
                {
                    FacingRight = !FacingRight;
                    transform.GetComponent<SpriteRenderer>().flipX = false; // flip to face Right
                }
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
            MoveWithSmoothDamp(velocityMult); // TODO: user Lerp?? 
        }

        public virtual void Jump(float forceMult = 1)
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
            _rb2D.AddForce(new Vector2(0, _jumpForce * forceMult), ForceMode2D.Impulse);
        }
        public virtual void Ghost()
        {
            GameManager.IsPlayerGhosted = !GameManager.IsPlayerGhosted;
        }
        private class PlayerGhostBehaviour : GhostBehaviour
        {
            private readonly Renderer _renderer;

            public PlayerGhostBehaviour(Renderer renderer)
            {
                _renderer = renderer;
            }
            protected override void OnGhostSet()
            {
                var col = _renderer.material.color;
                col.a = 0.5f;
                _renderer.material.color = col;
            }
            protected override void OnGhostUnset()
            {
                var col = _renderer.material.color;
                col.a = 1f;
                _renderer.material.color = col;
            }
        }

        // int animLayer = 0;
        // public bool isPlaying(string stateName)
        // {
        //     if (_animator.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
        //             _animator.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        //         return true;
        //     else
        //         return false;
        // }
        public virtual void Shoot()
        {
            if (isShooting) return;
            {
                isShooting = true;
                StartCoroutine(DelayArrow());

                IEnumerator DelayArrow() // delays the user from shooting every 'ShootDelay' seconds.
                {
                    //Debug.Log("Arrow is loading...");
                    yield return new WaitForSeconds(ShootDelaySeconds);
                    _clickSpawn.spawnObject();
                    isShooting = false;
                }
            }


        }

    }
}


