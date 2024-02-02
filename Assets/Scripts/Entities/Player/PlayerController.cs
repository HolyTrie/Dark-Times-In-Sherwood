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
        [SerializeField] private float _jumpForce = 15f;
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _runSpeedMult = 1.75f;
        public float RunSpeedMult { get { return _runSpeedMult; } }
        [SerializeField] private float _movementSmoothing = 0.35f;
        [SerializeField] private LayerMask _whatIsGround;							// A mask determining what is ground to the character
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider to be disabled on the 'crouch' player action.
        [SerializeField] private GameObject _groundCheck;							// A position marking where to check if the entity is grounded.
        [SerializeField] private Transform _ceilingCheck;							// A position marking where to check for ceilings
        [SerializeField] private float ShootLoad;
        [SerializeField] private float ShootReload;
        private bool _facingRight = true;                         // A boolean marking the entity's orientation.
        private Rigidbody2D _rb2D;                         // for manipulating an entity's physics by an IEntityMovement
        public Vector3 Velocity { get { return _rb2D.velocity; } }
        public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
        private Vector3 _Velocity = Vector3.zero;                // Entitys current velocity as a 3D vector. 
        private Animator _animator;
        public Animator Animator { get { return _animator; } }
        private ClickSpawn _clickSpawn; // class to spawn object by click.
        private Transform _transform;
        private bool isShooting = false;
        private Camera _mainCamera;
        private Renderer _renderer;
        private PlayerGhostBehaviour _gb;
        private GroundCheck _gc;
        public bool IsGrounded { get { return _gc.Grounded; } }
        void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _transform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
            _clickSpawn = GameObject.FindGameObjectWithTag("AttackPosRef").GetComponent<ClickSpawn>(); // TODO: fix magic strings
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // is used to check where the player is looking at if we shoot, so we flip it.
            _gc = GameObject.FindGameObjectWithTag("FloorCheck").GetComponent<GroundCheck>();
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
        protected virtual void Flip()
        {
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool flipRight = _rb2D.velocity.x <= 0 || mouseWorldPosition.x <= transform.position.x; // mushlam
            bool flipLeft = _rb2D.velocity.x > 0 || mouseWorldPosition.x > transform.position.x; // WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF

            if (_facingRight && flipRight)
            {
                _facingRight = !_facingRight;
                // _transform.localScale = Vector3.Scale(_transform.localScale, new Vector3(-1,1,1)); //legacy flip
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (!_facingRight && flipLeft)
            {
                _facingRight = !_facingRight;
                transform.GetComponent<SpriteRenderer>().flipX = false;
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
            private Renderer _renderer;

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
            isShooting = true;
            this.StartCoroutine(DelayArrow());
            
        }
        // delays the user from shooting every 'ShootDelay' seconds.
        private IEnumerator DelayArrow()
        {
            Debug.Log("Arrow is loading...");
            yield return new WaitForSeconds(ShootLoad);
            _clickSpawn.spawnObject();
            // yield return new WaitForSeconds(ShootReload);
            isShooting = false;
        }
    }
}


