using System;
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
    public class PlayerController : PhysicsObject2D
    {
        #region PLATFORMS
        public bool PassingThroughPlatform { get { return _passingThroughPlatform; } set { _passingThroughPlatform = value; } }
        [Header("Platforms")]
        [SerializeField]
        protected private LayerMask _whatIsPlatform;
        protected private ContactFilter2D _groundOnlyFilter;
        protected private ContactFilter2D _groundAndPlatformFilter;
        private bool _passingThroughPlatform = false;
        private Collider2D _previousPlatformCollider = null;
        public Collider2D PrevPlatformCollider { get { return _previousPlatformCollider; } set { _previousPlatformCollider = value; } }
        public int WhatIsPlatform { get { return _whatIsPlatform; } }
        #endregion

        #region INITIALIZATION
        private static PlayerController _Instance;
        public static PlayerController Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance.name = _Instance.GetType().ToString(); // name it for easy recognition
                    DontDestroyOnLoad(_Instance.gameObject); // mark root as DontDestroyOnLoad();
                }
                return _Instance;
            }
        }
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
            _hpBar = GetComponent<HpBarPlayer>();
            _playerGhostBehaviour = new PlayerGhostBehaviour(_spriteRenderer, _sanityBar, _ghostedSanityCost);
        }
        protected override void Start()
        {
            GameManager.SetController(this);
            base.Start();
            _baseGravity = Physics2D.gravity; // storing unitys gravity vector if its ever needed
            _originalGravity = _baseGravity;
            _currGravity = _originalGravity;
            _slopeGravity = _originalGravity * 2f;
            _isRunning = false;
            _wasRunning = false;
            _animator.speed = _playbackSpeed;
            if (_dashLengthRef != null)
            {
                _dashDistance = Vector2.Distance(transform.position, _dashLengthRef.transform.position);
            }
            InitJumpParams();
        }
        void InitJumpParams()
        {
            _isInPeakHang = false;
            _fallGravity = _baseGravity * _fallGravityMult;
            var Height = Vector2.Distance(transform.position, _jumpVerticalPeak.position); // h
            var HorizontalDistToPeak = Vector2.Distance(transform.position, _jumpHorizontalPeak.position); // X_h
            var Vx = _walkSpeed;
            var Th = HorizontalDistToPeak / Vx;
            _timeToJumpPeak = Th;
            _jumpForce = 2 * Height / Th;
            _jumpGravity = -2 * Height / (Th * Th);
            Debug.Log($"initial jump force = {_jumpForce} | jump gravity = {_jumpGravity}");
            // strong jump
            Height = Vector2.Distance(transform.position, _strongJumpVerticalPeak.position); // h
            HorizontalDistToPeak = Vector2.Distance(transform.position, _strongJumpHorizontalPeak.position); // X_h
            Vx = _walkSpeed * _runSpeedMult;
            Th = HorizontalDistToPeak / Vx;
            _strongJumpForce = 2 * Height / Th;
            _strongJumpGravity = -2 * Height / (Th * Th);
            Debug.Log($"strong jump force = {_strongJumpForce} | strong jump gravity = {_strongJumpGravity}");
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _groundOnlyFilter.useTriggers = false;
            _groundAndPlatformFilter.useTriggers = false;
            _groundOnlyFilter.SetLayerMask(_whatIsGround);
            _groundAndPlatformFilter.SetLayerMask(_whatIsGround + _whatIsPlatform);
            _groundOnlyFilter.useLayerMask = true;
            _groundAndPlatformFilter.useLayerMask = true;
            _currFilter = _groundAndPlatformFilter;
        }
        #endregion

        #region CHECKS
        //TODO: automatic init instead of serialize field. idea: have a script in the checks object that iterates on children and adds them here.
        public bool IsGrounded { get { return _gc.Grounded(); } }
        public CeilingCheck CeilingCheck { get { return _ceilingCheck; } }
        [SerializeField] private GroundCheck _gc;
        [SerializeField] private SlopeCheck _sc;
        [SerializeField] private HorizontalCollisionCheck2D _hc;
        [SerializeField] private PlatformCheck _platformCheck;
        [SerializeField] private CeilingCheck _ceilingCheck;
        #endregion

        #region GENERAL
        private PlayerStateMachine _fsm;
        public PlayerStateMachine FSM { get { return _fsm; } internal set { _fsm = value; } } // TODO: refactor to remove this it makes no sense.
        public int GroundOnlyLayerMask { get { return _whatIsGround; } }
        public Vector2 CurrGravity { get { return _currGravity; } set { _currGravity = value; } }
        public Vector2 OriginalGravity { get { return _originalGravity; } private set { _originalGravity = value; } }
        private Vector2 _originalGravity;
        private bool _facingRight = true; // A boolean marking the entity's orientation.
        public bool FacingRight { get { return _facingRight; } private set { _facingRight = value; } }
        public Vector2 Position { get { return transform.position; } }

        /* *** CAMERA*** */
        private Camera _mainCamera;

        #endregion

        #region FLIPS
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
                    // _spriteRenderer.flipX = true; // flip to face Left
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                if (!FacingRight && movingRight)
                {
                    FacingRight = !FacingRight;
                    // _spriteRenderer.flipX = false; // flip to face Right
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        public void FlipByCursorPos()
        {
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool isMouseRightToPlayer = mouseWorldPosition.x > transform.position.x;
            bool isMouseLeftToPlayer = mouseWorldPosition.x < transform.position.x;
            if (FacingRight && isMouseLeftToPlayer)
            {
                FacingRight = !FacingRight;
                // _spriteRenderer.flipX = true; // flip to face Left
                transform.localScale = new Vector3(-1, 1, 1);

            }
            if (!FacingRight && isMouseRightToPlayer)
            {
                FacingRight = !FacingRight;
                // _spriteRenderer.flipX = false; // flip to face Right
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        #endregion

        #region WALK & RUN
        public void Move(Vector2 move)
        {
            var tmp = move * _walkSpeed;
            _targetVelocity = new(tmp.x, tmp.y);
        }
        public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }
        public float RunSpeedMult { get { return _runSpeedMult; } }
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
        public float WalkSpeed { get { return _walkSpeed; } }

        [Header("Player Physics")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeedMult;
        private bool _isRunning;
        private bool _wasRunning;
        public bool WasRunning { get { return _wasRunning; } set { _wasRunning = value; } }
        #endregion

        #region SLOPE 
        public Vector2 SlopeGravity { get { return _slopeGravity; } private set { _slopeGravity = value; } }
        Vector2 _slopeGravity;
        public bool SlopeAhead
        {
            get
            {
                bool ans;
                if (_facingRight)
                {
                    ans = _sc.SlopeAhead || _hc.RightCollisionType == HorizontalCollisionCheck2D.CollisionTypes.PARTIAL;
                }
                else
                {
                    ans = _sc.SlopeAhead || _hc.LeftCollisionType == HorizontalCollisionCheck2D.CollisionTypes.PARTIAL;
                }
                return ans;
            }
        }
        public bool IsSlopeUpwardsLeftToRight { get { return _sc.IsSlopeUpwardsLeftToRight; } }
        #endregion

        #region JUMP & GRAVITY
        /// <summary>
        /// Sets _isJumping=true and gravity to jumping or strong jumping gravity.
        /// </summary> <summary>
        /// 
        /// </summary>
        public void Jump(bool weakJump = false)
        {
            _isJumping = true;
            var jumpForce = _jumpForce;
            var gravity = _jumpGravity;
            if (_wasRunning)
            {
                jumpForce = _strongJumpForce;
                gravity = _strongJumpGravity;
            }
            CurrGravity = new(0f, gravity);
            var velMult = 1f;
            if(weakJump) velMult*=0.8f;
            _velocity.y = velMult * jumpForce;
        }
        public void AccelarateFall()
        {
            CurrGravity = new(0f, CurrGravity.y * _fallGravityMult);
        }
        public bool JumpWasBuffered { get; set; }
        public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
        public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
        public bool IsInPeakHang { get { return _isInPeakHang; } set { _isInPeakHang = value; } }
        public Vector2 FallGravity { get { return _fallGravity; } set { _fallGravity = value; } }
        public float JumpPeakHangThreshold { get { return _jumpPeakHangThreshold; } }
        public float JumpPeakGravityMult { get { return _gravityMultAtPeak; } }
        public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
        public float CoyoteTime { get { return _coyoteTime; } set { _coyoteTime = value; } }
        public float JumpBufferTime { get { return _jumpBufferTime; } set { _jumpBufferTime = value; } }
        public float JumpBufferCounter { get { return _jumpBufferCounter; } set { _jumpBufferCounter = value; } }
        public float StickyFeetDuration { get { return _stickyFeetDuration; } set { _stickyFeetDuration = value; } }
        public float StickyFeetFriction { get { return 1f - _stickyFeetFriction; } set { _stickyFeetFriction = value; } }
        public bool StickyFeetConsidersDirection { get { return _stickyFeetConsidersDirection; } set { _stickyFeetConsidersDirection = value; } }
        public bool IsInStickyFeet { get; set; }
        public bool StickyFeetDirectionIsRight { get; set; }

        [Header("Jump Parameters")]
        [SerializeField, Range(0, 1), Tooltip("how much of the body % must touch the top for bumping head correction, 0 = correction happens all the time and 1 = no correction")]
        private float _headBumpCorrectionThreshold = 0.5f;

        [SerializeField, Tooltip("how much time in seconds is the sticky feet effect active when landing on a new platform")]
        private float _stickyFeetDuration = 0.25f;
        [SerializeField, Range(0, 1), Tooltip("how much to slow down the movement, 1 = full stop, 0 = no effect")]
        private float _stickyFeetFriction = 0.90f;
        [SerializeField, Tooltip("if set to false sticky feet effect will happen in both direction when landing on a platform, otherwise the effect will work only in the OPPOSITE direction of the jump when landing on a platform")]
        private bool _stickyFeetConsidersDirection = true;
        [SerializeField]
        private Transform _jumpVerticalPeak;
        [SerializeField]
        private Transform _jumpHorizontalPeak;
        [SerializeField]
        private Transform _strongJumpVerticalPeak;
        [SerializeField]
        private Transform _strongJumpHorizontalPeak;
        [SerializeField]
        private float _maxFallSpeed = 25f;
        [SerializeField, Range(0f, 1f)]
        private float _gravityMultAtPeak = 0.25f;
        [SerializeField]
        private float _jumpPeakHangThreshold;
        [SerializeField]
        private float _fallGravityMult = 1.5f;
        [SerializeField]
        private float _weakJumpGravityMult = 2f;
        [Tooltip("The time in seconds that the player has to perfrom a free jump mid-air, gives players some grace time to perform their jump even if they werent robot-precise with it")]
        [SerializeField]
        private float _coyoteTime = 0.5f;
        [Tooltip("The time in seconds that the game will buffer a jump if it was pressed too soon before landing, gives players who press jump too early more freedom")]
        [SerializeField]
        private float _jumpBufferTime = 0.25f;
        private bool _isJumping = false;
        private bool _isFalling = false;
        private bool _isInPeakHang = false;
        private float _jumpBufferCounter = 0;
        private float _jumpForce;
        private float _timeToJumpPeak;
        private float _jumpGravity;
        private float _strongJumpForce;
        private float _timeToStrongJumpPeak;
        private float _strongJumpGravity;
        private Vector2 _baseGravity;
        private Vector2 _currGravity;
        private Vector2 _fallGravity;
        #endregion

        #region ANIMATION
        [Header("Animation")]
        [SerializeField][Range(0, 1)] private float _playbackSpeed = 1f;
        private Animator _animator;
        public Animator Animator { get { return _animator; } }

        /* *** PARTICLES *** */
        private TrailRenderer _tr;
        #endregion

        #region PLAYER ATTRIBUTES
        [Header("Player Attributes")]
        public int _jumpStaminaCost;
        public int _ghostedSanityCost;
        [Header("Environmentals Checkers")]
        // [SerializeField] private bool _airControl = true;
        [SerializeField] private Collider2D _CrouchDisableCollider;    // A collider to be disabled on the 'crouch' player action.

        [Header("Shooting")]
        [SerializeField] private float ShootDelaySeconds;
        [SerializeField] private float ShootReloadSeconds;
        #endregion

        #region STATBARS
        /* *** STAMINA *** */
        private StaminaBar _staminabar;
        public StaminaBar StaminaBar { get { return _staminabar; } }

        /* *** SANITY *** */
        private SanityBar _sanityBar;
        public SanityBar SanityBar { get { return _sanityBar; } }
        /* *** HP *** */
        private HpBarPlayer _hpBar;
        public HpBarPlayer HpBar { get { return _hpBar; } }
        #endregion

        #region GHOST MECHANIC
        public virtual void Ghost()
        {
            GameManager.IsPlayerGhosted = !GameManager.IsPlayerGhosted;
        }
        private PlayerGhostBehaviour _playerGhostBehaviour;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion

        #region SHOOTING
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private bool _isShooting = false;
        public bool isShooting { get { return _isShooting; } set { _isShooting = value; } }
        #endregion

        #region DASH
        public void Dash()
        {
            if (_canDash)
            {
                var direction = _facingRight == true ? Vector2.right : Vector2.left;
                var hit = Physics2D.Raycast(transform.position, direction, _dashDistance, _currFilter.layerMask);
                var distance = Vector2.Distance(transform.position, hit.point);
                if (distance < _dashDistance)
                {
                    _dashDistance = distance;
                }
                StartCoroutine(StartDash(_gravityModifier));
            }
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
        }
        [Header("Dash Settings")]
        //[SerializeField] private int _ConsecutiveDashes = 2;
        //[SerializeField] private float _ConsecutiveDashTimeframe = 0.5f;
        [SerializeField, Range(0.001f, 5)] private float _dashDurationSeconds = 0.2f;
        [SerializeField] private Transform _dashLengthRef;
        [SerializeField] private float _dashCooldown = 1f;
        private float _dashDistance = 5f;
        private bool _canDash = true;
        private bool _isDashing = false;

        #endregion

        #region UPDATES & PHYSICS

        public void NudgeToPosition(Vector3 position)
        {
            // todo: move without tunelling
            // note : always moving to empty air so???
            _rb2d.MovePosition(position);
            Jump();
        }
        public void AddForce(Vector2 force)
        {
            _targetVelocity += force;
        }
        protected private override void Update()
        {
            //base.Update();
            _playerGhostBehaviour.TrySetGhostStatus();
            Flip();
        }
        protected private override void FixedUpdate()
        {
            if (_isDashing)
            {
                _velocity = new(0f, 0f);
                var direction = _facingRight ? 1.0f : -1.0f;
                var velocity = _dashDistance / _dashDurationSeconds;// S = V * T --> S/T = V
                _gravityModifier = 0f; //re applied by a corutine!
                _targetVelocity = new(direction * velocity, 0f);
            }
            _grounded = false;
            _onSlope = false;
            var acc = Time.deltaTime * _gravityModifier * CurrGravity;

            _velocity += acc; // apply gravity to the objects velocity
            _velocity.x = _targetVelocity.x;
            Vector2 deltaPosition = _velocity * Time.deltaTime;
            Vector2 moveAlongGround = new(_groundNormal.y, -_groundNormal.x); //helps with slopes  
            Vector2 moveX = moveAlongGround * deltaPosition;
            Vector2 moveY = Vector2.up * deltaPosition.y;
            List<Collider2D> colliderToIgnore = new(16); // todo: variable size?
            if (_passingThroughPlatform)
                colliderToIgnore.Add(_platformCheck.Curr != null ? _platformCheck.Curr.Collider : null);
            Movement(moveY, true, colliderToIgnore); // vertical movement
            Movement(moveX, false, colliderToIgnore); // horizontal movement
            _velocity.y = Math.Clamp(_velocity.y, -_maxFallSpeed, float.MaxValue);
            _targetVelocity = Vector2.zero;
            // predict future position using a simplified euler integration (0.5 pixel error rate, resets when landing so it does not accumulate!)
            var futurePos = (Vector2)transform.position + _velocity * Time.deltaTime + 0.5f * Time.deltaTime * acc; // pos = velocity*deltaTime +0.5*accelaration*(deltaTime^2)
            var futureVel = _velocity + acc; // vel = accelaration * deltaTime & acceleration = gravity only rn.
            var futurePosTop = futurePos;
            futurePosTop.y += _collider.bounds.size.y;
            var hit = Physics2D.Raycast(futurePosTop,Vector2.up,0.01f,WhatIsGround);
            if(!_isNudging && !hit && futureVel.y < 0 && IsJumping && _ceilingCheck.CollisionType != CeilingCheck.CollisionTypes.NONE)
            {
                bool NudgeLeft = futurePos.x > futurePosTop.x;
                if(_collider.bounds.center.y < futurePosTop.y)
                    StartCoroutine(JumpNudge(NudgeLeft));
                //else
                    //StartCoroutine(GrabLedge(NudgeLeft));
            }
        }
        private bool _isNudging = false;
        private IEnumerator JumpNudge(bool jumpingLeft)
        {
            _isNudging = true;
            Debug.Log($"nudge | curr velocity = {_velocity}");
            Vector2 pos = new(_rb2d.position.x ,_rb2d.position.y);
            Vector2 offset = new(_collider.bounds.extents.x+0.001f,0.001f);
            if(jumpingLeft && !FacingRight)
                offset.x *= -1f;
            pos += offset;
            _rb2d.MovePosition(pos);
            Jump(weakJump: true);
            yield return new WaitForSeconds(1f);
            _isNudging = false;
        }
        protected private void Movement(Vector2 move, bool yMovement, List<Collider2D> collidersToIgnore)
        {
            float distance = move.magnitude;
            if (distance > _minMoveDistance)
            {
                int count = _rb2d.Cast(move, _currFilter, _hitBuffer, distance + _shellRadius); // stores results into _hitBuffer and returns its length (can be discarded).
                _hitBufferList.Clear();
                if (count > 0)
                {
                    _hitBufferList.Add(_hitBuffer[0]);
                }
                foreach (var hit in _hitBufferList)
                {
                    if (collidersToIgnore.Contains(hit.collider)) // added support to ignore specified colliders, for example platforms
                        continue;
                    Vector2 currentNormal = hit.normal;
                    if (currentNormal.y > _minGroundNormalY) // if the normal vectors angle is greater then the set value.
                    {
                        _grounded = true;
                        if (yMovement)
                        {
                            _groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    float projection = Vector2.Dot(_velocity, currentNormal);
                    if (projection < 0)
                    {
                        _velocity -= projection * currentNormal; // cancel out the velocity that would be lost on impact.
                    }

                    float modifiedDistance = hit.distance - _shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            var distanceToMove = move.normalized * distance;
            var pos = _rb2d.position + distanceToMove;
            _rb2d.position = pos;
            //_rb2d.MovePosition(pos);
        }
        #endregion

        #region DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector2 moveAlongGround = new(_groundNormal.y, -_groundNormal.x);
            //Gizmos.DrawRay(transform.position,moveAlongGround);
        }
        #endregion
    }
}


