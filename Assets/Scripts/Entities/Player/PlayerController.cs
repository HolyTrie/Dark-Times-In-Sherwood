using System;
using System.Collections;
using UnityEngine;

namespace DTIS
{
    /*
        Responsible for all 'basic' aspects of entity management in unity like position and physics, acting as a template class for other classes to override and specialize.
        Important reference:
        - https://stackoverflow.com/questions/12662072/what-is-protected-virtual-new
    */
    public class PlayerController : PhysicsObject
    {
        [Header("Animation")]
        [SerializeField][Range(0,1)] private float _playbackSpeed = 1f;
        [Header("Player Physics")]
        [SerializeField] private float _jumpForce;
        public float JumpForce{get{return _jumpForce;}set{_jumpForce = value;}}
        [SerializeField] private float _fallGravityMult = 2.5f;
        [SerializeField] private float _weakJumpGravityMult = 2f;
        [SerializeField] private float _walkSpeed;
        public float WalkSpeed{get{return _walkSpeed;}}
        [SerializeField] private float _runSpeedMult;
        public float RunSpeedMult{get{return _runSpeedMult;}}
        public Vector2 Velocity{get{return _velocity;}set{_velocity = value;}}

        [Header("Player Attributes")]
        public int _jumpStaminaCost;
        public int _ghostedSanityCost;

        [Header("Environmentals Checkers")]
        // [SerializeField] private bool _airControl = true;
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider to be disabled on the 'crouch' player action.
        //[SerializeField] private Transform _ceilingCheck;                           // A position marking where to check for ceilings

        [Header("Shooting")]
        [SerializeField] private float ShootDelaySeconds;
        [SerializeField] private float ShootReloadSeconds;

        //Player related vars//
        private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        public bool FacingRight { get { return _facingRight; } private set { _facingRight = value; } }

        /* *** ANIMATOR *** */
        private Animator _animator;
        public Animator Animator { get { return _animator; } }
        
        /* *** PARTICLES *** */
        private TrailRenderer _tr;

        /* *** FSM *** */
        private PlayerStateMachine _fsm;
        public PlayerStateMachine FSM { get { return _fsm; } internal set { _fsm = value; } } // TODO: refactor to remove this it makes no sense.

        /* *** GROUND CHECK*** */
        [SerializeField] private GroundCheck _gc;
        public bool IsGrounded { get { return _gc.Grounded(); } }

        /* *** STAMINA *** */
        private StaminaBar _staminabar;
        public StaminaBar StaminaBar { get { return _staminabar; } }

        /* *** SANITY *** */
        private SanityBar _sanityBar;
        public SanityBar SanityBar { get { return _sanityBar; } }

        /* *** GHOST MECHANIC *** */
        private PlayerGhostBehaviour _playerGhostBehaviour;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        /* *** SHOOTING */
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private bool isShooting = false;

        /* *** CAMERA*** */
        private Camera _mainCamera;

        /* *** PLATFORMS *** */
        private bool _passingThroughPlatform = false;
        private LayerMask _initialGroundLayerMask;
        public bool PassingThroughPlatform{get{return _passingThroughPlatform;}private set{_passingThroughPlatform=value;}}
        public void SetPassingThroughPlatform(bool value)
        {
            if(value)
            {
                _contactFilter2d.SetLayerMask(_groundOnlyFilter.layerMask);
            }
            else
            {
                _contactFilter2d.SetLayerMask(_initialGroundLayerMask);
            }
            PassingThroughPlatform = true;
        }

        /* *** DASH *** */
        [Header("Dash Settings")]
        private bool _canDash = true;
        private bool _isDashing = false;
        [SerializeField, Range(0.001f, 5)] private float _dashDurationSeconds = 0.2f;
        [SerializeField] private Transform _dashLengthRef;
        private float _dashDistance = 5f;
        [SerializeField] private float _dashCooldown = 1f;
        //[SerializeField] private int _ConsecutiveDashes = 2;
        //[SerializeField] private float _ConsecutiveDashTimeframe = 0.5f;

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _tr = GetComponent<TrailRenderer>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _gc = GetComponentInChildren<GroundCheck>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _staminabar = GetComponent<StaminaBar>();
            _sanityBar = GetComponent<SanityBar>();
            _playerGhostBehaviour = new PlayerGhostBehaviour(_spriteRenderer, _sanityBar, _ghostedSanityCost);
        }
        void Start()
        {
            _initialGroundLayerMask = _contactFilter2d.layerMask;
            _animator.speed = _playbackSpeed;
            if(_dashLengthRef != null) 
            {
                _dashDistance = Math.Abs(Vector2.Distance(transform.position,_dashLengthRef.transform.position));
            }
        }
        void Update()
        {
            _playerGhostBehaviour.TrySetGhostStatus();
            Flip();
        }

        /*Flips the chacater according to his velocity*/
        protected virtual void Flip(bool overrideMovement = false)
        {
            if (_velocity.x != 0) // if not idle
            {
                bool movingRight = _velocity.x > 0;
                bool movingLeft = _velocity.x < 0;
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
        private void FlipByCursorPos()
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

        public void Move(Vector2 move)
        {
            _targetVelocity = move * _walkSpeed;
        }

        public void Jump()
        {
            _velocity.y = _jumpForce;
        }

        internal void AccelarateFall()
        {
            _velocity += (_fallGravityMult - 1) * Time.deltaTime * Physics2D.gravity * Vector2.up;
        }

        public void Dash()
        {
            if(_canDash)
                StartCoroutine(StartDash(_gravityModifier));
        }
        private protected override void FixedUpdate() // fully overriden to support more complex behaviour like dashing.
        {
            if(_isDashing)
            {
                _velocity = new(0f,0f);
                var direction = _facingRight ? 1.0f : -1.0f;
                var velocity = _dashDistance / _dashDurationSeconds;// S = V * T --> S/T = V
                _gravityModifier = 0f;
                _targetVelocity = new(direction * velocity,0f);
            }
            base.FixedUpdate();
        }
        private IEnumerator StartDash(float OriginalGravityModifier)
        {
            _canDash = false;
            _isDashing = true;
            _tr.emitting = true;
            yield return new WaitForSeconds(_dashDurationSeconds);
            _tr.emitting = false;
            _gravityModifier = OriginalGravityModifier;
            _isDashing = false;
            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
            _velocity.x = 0f;
        }
    }
}


