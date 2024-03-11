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
            GetCheckComponents();
            _animator = GetComponentInChildren<Animator>();
            _tr = GetComponent<TrailRenderer>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _staminabar = GetComponent<StaminaBar>();
            _sanityBar = GetComponent<SanityBar>();
            _hpBar = GetComponent<HpBarPlayer>();
            _playerGhostBehaviour = new PlayerGhostBehaviour(_spriteRenderer, _sanityBar, _ghostedSanityCost);
            _cameraFollowObject = _cameraFollow.GetComponent<CameraFollowObject>();
        }
        protected override void Start()
        {
            GameManager.SetController(this);
            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
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

        #region PLATFORMS
        public bool PassingThroughPlatform { get { return _passingThroughPlatform; } set { _passingThroughPlatform = value; } }
        public Collider2D PrevPlatformCollider { get { return _previousPlatformCollider; } set { _previousPlatformCollider = value; } }
        public int WhatIsPlatform { get { return _whatIsPlatform; } }

        [Header("Platforms")]
        [SerializeField]
        protected private LayerMask _whatIsPlatform;
        protected private ContactFilter2D _groundOnlyFilter;
        protected private ContactFilter2D _groundAndPlatformFilter;
        private Collider2D _previousPlatformCollider = null;
        private bool _passingThroughPlatform = false;
        #endregion

        #region LEDGE GRAB
        public void GrabLedgeFromBelow(Collider2D collider)
        {
            FSM.SetStates(ESP.States.Airborne,ESP.States.LedgeGrabState);
            var colliderBotY = collider.bounds.min.y;
            var playerTopY = _collider.bounds.max.y;
            var distance = Mathf.Abs(colliderBotY - playerTopY);
            var pos = _rb2d.position;
            pos.y += distance;
            _rb2d.position = pos;
        }

        public void GrabLedgeFromAbove(Collider2D collider)
        {
            var colliderBotY = collider.bounds.min.y;
            var playerTopY = _collider.bounds.max.y;
            var distance = Mathf.Abs(colliderBotY - playerTopY);
            StartCoroutine(TransitionBelowLedge(distance));
        }
        public IEnumerator TransitionBelowLedge(float distanceY)
        {
            float animSteps = 3f; //TODO - fix magic num
            distanceY /= animSteps;
            var pos = _rb2d.position;
            pos.y -= distanceY;
            _rb2d.position = pos;
            for(int i = 0; i < animSteps - 1; ++i)
            {
                yield return new WaitForSeconds(0.05f);
                pos.y -= distanceY;
                _rb2d.position = pos;
            }
            FSM.SetStates(ESP.States.Airborne,ESP.States.LedgeGrabState);
        }
        public bool GrabbingLedge { get { return _grabbingLedge; } set { _grabbingLedge = value; } }
        public bool LeavingLedge { get { return _leavingLedge; } set { _leavingLedge = value; } }
        private bool _leavingLedge = false;
        private bool _grabbingLedge = false;
        #endregion

        #region CHECKS
        //TODO: automatic init instead of serialize field. idea: have a script in the checks object that iterates on children and adds them here.
        public bool IsGrounded { get { return _groundCheck.Grounded(); } }
        public CeilingCheck CeilingCheck { get { return _ceilingCheck; } }
        public HorizontalCollisionCheck2D HorizontalCheck { get { return _horizontalCheck; } }
        public bool EdgeAhead { get { return _edgeLocator.Hit; } }
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private SlopeCheck _slopeCheck;
        [SerializeField] private HorizontalCollisionCheck2D _horizontalCheck;
        [SerializeField] private PlatformCheck _platformCheck;
        [SerializeField] private CeilingCheck _ceilingCheck;
        [SerializeField] private EdgeLocator _edgeLocator;
        private void GetCheckComponents()
        {
            _groundCheck = GetComponentInChildren<GroundCheck>();
            _slopeCheck = GetComponentInChildren<SlopeCheck>();
            _ceilingCheck = GetComponentInChildren<CeilingCheck>();
            _platformCheck = GetComponentInChildren<PlatformCheck>();
            _horizontalCheck = GetComponentInChildren<HorizontalCollisionCheck2D>();
            _edgeLocator = GetComponentInChildren<EdgeLocator>();
        }
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
        [Header("Camera")]
        [SerializeField] private GameObject _cameraFollow;
        private Camera _mainCamera;
        private CameraFollowObject _cameraFollowObject;

        private void CameraLerp()
        {
            //handles the camera falls//

            //if we are falling past a certain speed thershold 
            if (_velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpYDamping(true);
            }

            //if we are standing still or moving up
            if (_velocity.y >= 0 && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
            {
                //reset so it can be called again
                CameraManager.instance.LerpedFromPlayerFalling = false;
                CameraManager.instance.LerpYDamping(false);
            }
        }
        #endregion

        #region PlayerFX
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
                    // _spriteRenderer.flipX = true; // flip to face Left
                    // transform.localScale = new Vector3(-1, 1, 1); // old 

                    Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                    FacingRight = !FacingRight;

                    //turn the camera follow object
                    _cameraFollowObject.Flip();
                }
                if (!FacingRight && movingRight)
                {
                    // _spriteRenderer.flipX = false; // flip to face Right
                    // transform.localScale = new Vector3(1, 1, 1); // old

                    Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                    FacingRight = !FacingRight;

                    //turn the camera follow object
                    _cameraFollowObject.Flip();
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
                // _spriteRenderer.flipX = true; // flip to face Left
                // transform.localScale = new Vector3(-1, 1, 1);

                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                FacingRight = !FacingRight;

            }
            if (!FacingRight && isMouseRightToPlayer)
            {
                // _spriteRenderer.flipX = false; // flip to face Right
                // transform.localScale = new Vector3(1, 1, 1);

                Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                FacingRight = !FacingRight;
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
        private float _fallSpeedYDampingChangeThreshold; // for camera follow when player falls

        #endregion

        #region SLOPE 
        public Vector2 SlopeGravity { get { return _slopeGravity; } private set { _slopeGravity = value; } }
        Vector2 _slopeGravity;
        public bool SlopeAhead
        {
            get
            {
                bool ans = _slopeCheck.SlopeAhead || _horizontalCheck.CollisionType == HorizontalCollisionCheck2D.CollisionTypes.PARTIAL;
                return ans;
            }
        }
        public bool IsSlopeUpwardsLeftToRight { get { return _slopeCheck.IsSlopeUpwardsLeftToRight; } }
        #endregion

        #region JUMP & GRAVITY
        /// <summary>
        /// Sets _isJumping=true and gravity to jumping or strong jumping gravity.
        /// </summary> <summary>
        /// 
        /// </summary>
        public void Jump(bool weakJump = false)
        {
            // ParticleSystemManager.instance.playJumpEffect();

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
            if (weakJump) velMult *= 0.8f;
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
                ParticleSystemManager.instance.playDashEffect();

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
            // _tr.emitting = true;
            yield return new WaitForSeconds(_dashDurationSeconds);
            // _tr.emitting = false;
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
        public Vector2 FuturePos { get { return _futurePosition; } }
        public Vector2 FutureVel { get { return _futureVelocity; } }
        private Vector2 _futurePosition;
        private Vector2 _futureVelocity;
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
            CameraLerp();
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
            {
                //colliderToIgnore.Add(_platformCheck.Curr != null ? _platformCheck.Curr.Collider : null);
                _currFilter = _groundOnlyFilter;
            }
            else
                _currFilter = _groundAndPlatformFilter;
            Movement(moveY, true, colliderToIgnore); // vertical movement
            Movement(moveX, false, colliderToIgnore); // horizontal movement
            _velocity.y = Math.Clamp(_velocity.y, -_maxFallSpeed, float.MaxValue);
            _targetVelocity = Vector2.zero;

            // predict future position using a simplified euler integration (0.5 pixel error rate, resets when landing so it does not accumulate!)
            _futurePosition = (Vector2)transform.position + _velocity * Time.deltaTime + 0.5f * Time.deltaTime * acc; // pos = velocity*deltaTime +0.5*accelaration*(deltaTime^2)
            _futureVelocity = _velocity + acc; // vel = accelaration * deltaTime & acceleration = gravity only rn.

            // Bumped Head Correction
            var futurePosTop = _futurePosition;
            futurePosTop.y += _collider.bounds.size.y;
            var hit = Physics2D.Raycast(futurePosTop, Vector2.up, 0.01f, WhatIsGround);
            var bumpedHeadCorrection = !_isNudging && !hit && _futureVelocity.y < 0 && IsJumping && _ceilingCheck.CollisionType != CeilingCheck.CollisionTypes.NONE;
            if (bumpedHeadCorrection)
            {
                bool NudgeLeft = _futurePosition.x > futurePosTop.x;
                if (_collider.bounds.center.y < futurePosTop.y)
                    StartCoroutine(JumpNudge(NudgeLeft));
            }
        }
        private bool _isNudging = false;
        private IEnumerator JumpNudge(bool jumpingLeft)
        {
            _isNudging = true;
            Debug.Log($"nudge | curr velocity = {_velocity}");
            Vector2 pos = new(_rb2d.position.x, _rb2d.position.y);
            Vector2 offset = new(_collider.bounds.extents.x + 0.001f, 0.001f);
            if (jumpingLeft && !FacingRight)
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
                        continue; //TODO: this idea causes tunneling as well
                    Vector2 currentNormal = hit.normal;
                    if (currentNormal.y > _minGroundNormalY) // if the normal vectors angle is greater then the min' value.
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
                    if(Util.IsPointInCollider(hit.point,hit.collider))
                    {
                        Debug.Log("tunneling detected");
                        //snap to collider position + _shellRadius
                        // distance = 0f?
                    }
                }
            }
            var distanceToMove = move.normalized * distance;
            var pos = _rb2d.position + distanceToMove;
            _rb2d.position = pos;
            //var pos = transform.position + (Vector3)distanceToMove;
            //transform.position = pos;
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


