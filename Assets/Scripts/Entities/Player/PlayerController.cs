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
        [Header("Player Forces")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeedMult;

        [Header("Player Attributes")]
        [SerializeField] public int _jumpStaminaCost;
        [SerializeField] public int _ghostedSanityCost;

        public float RunSpeedMult { get { return _runSpeedMult; } }

        [Header("Environmentals Checkers")]
        // [SerializeField] private bool _airControl = true;
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider to be disabled on the 'crouch' player action.
        [SerializeField] private Transform _ceilingCheck;                           // A position marking where to check for ceilings

        [Header("Smoothment")]
        [SerializeField] private float _movementSmoothing;
        [SerializeField] private float ShootDelaySeconds;
        [SerializeField] private float ShootReloadSeconds;

        //Player related vars//
        private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        public bool FacingRight { get { return _facingRight; } private set { _facingRight = value; } }

        private Rigidbody2D _rb2D;                         // for manipulating an entity's physics by an IEntityMovement
        public Vector3 Velocity { get { return _rb2D.velocity; } }
        public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

        private Animator _animator;
        public Animator Animator { get { return _animator; } }

        private PlayerStateMachine _fsm;
        public PlayerStateMachine FSM { get { return _fsm; } internal set { _fsm = value; } }

        private GroundCheck _gc;
        public bool IsGrounded { get { return _gc.Grounded(); } }

        private StaminaBar _staminabar;
        public StaminaBar StaminaBar { get { return _staminabar; } }

        private SanityBar _sanityBar;
        public SanityBar SanityBar { get { return _sanityBar; } }

        //Ghost player//
        private PlayerGhostBehaviour _playerGhostBehaviour;
        private SpriteRenderer _spriteRenderer;

        //Shooting vars//
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private bool isShooting = false;

        //Extra Vars//
        private Camera _mainCamera;
        private Vector3 _Velocity = Vector3.zero;  // Entitys current velocity as a 3D vector.

        void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _gc = GetComponentInChildren<GroundCheck>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _staminabar = GetComponent<StaminaBar>();
            _sanityBar = GetComponent<SanityBar>();
            _playerGhostBehaviour = new PlayerGhostBehaviour(_spriteRenderer, _sanityBar, _ghostedSanityCost);
        }

        // Update is called once per frame
        void Update()
        {
            _playerGhostBehaviour.TrySetGhostStatus();
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
                    _spriteRenderer.flipX = true; // flip to face Left
                }
                if (!FacingRight && isMouseRightToPlayer)
                {
                    FacingRight = !FacingRight;
                    _spriteRenderer.flipX = false; // flip to face Right
                }
            }
            else
            {
                bool movingRight = _rb2D.velocity.x > 0;
                bool movingLeft = _rb2D.velocity.x < 0;
                if (FacingRight && movingLeft)
                {
                    FacingRight = !FacingRight;
                    _spriteRenderer.flipX = true; // flip to face Left
                }
                if (!FacingRight && movingRight)
                {
                    FacingRight = !FacingRight;
                    _spriteRenderer.flipX = false; // flip to face Right
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
        

        //this method should check if a certain animation is still playing (like shooting, if so DO NOT SHOOT)
        public bool isPlaying(string stateName)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                    _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                return true;
            else
                return false;
        }
    }
}


