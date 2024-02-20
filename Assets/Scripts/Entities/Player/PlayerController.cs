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
        private Animator _animator;
        public Animator Animator { get { return _animator; } }

        private PlayerStateMachine _fsm;
        public PlayerStateMachine FSM { get { return _fsm; } internal set { _fsm = value; } } // TODO: refactor to remove this it makes no sense.

        [SerializeField] private GroundCheck _gc;
        public bool IsGrounded { get { return _grounded; } }
        //public bool IsGrounded { get { return _gc.Grounded(); } }
        private StaminaBar _staminabar;
        public StaminaBar StaminaBar { get { return _staminabar; } }

        private SanityBar _sanityBar;
        public SanityBar SanityBar { get { return _sanityBar; } }

        //Ghost player//
        private PlayerGhostBehaviour _playerGhostBehaviour;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        //Shooting vars//
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private bool isShooting = false;

        //Extra Vars//
        private Camera _mainCamera;
        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _gc = GetComponentInChildren<GroundCheck>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

        /*Flips the chacater according to his velocity*/
        protected virtual void Flip(bool overrideMovement = false)
        {
            if (_velocity.x == 0) // if idle
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
        public virtual void Ghost()
        {
            //Debug.Log("ghost callback");
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
        public void ResetVelocityY()
        {
            _velocity.y = 0f;
        }
    }
}


