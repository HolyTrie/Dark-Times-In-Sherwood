using System;
using System.Collections;
using UnityEngine;

public class PunchTrap : MonoBehaviour
{
    [SerializeField] private Transform _activeSprite;
    [SerializeField] private Transform _hitStart;
    [SerializeField] private Transform _hitEnd;
    [SerializeField] private float _force = 1000f;
    //private Vector3 _parentPos;
    //private float _distance;  // Amount to move left and right
	private float _speed; 
    //private Animator _animator;
    private float _direction;
    private CircleCollider2D _hitbox;
    //private Rigidbody2D _rb;
    void Start()
    {
        //_animator = GetComponent<Animator>();
        //_animator.StopPlayback();
        _hitbox = GetComponent<CircleCollider2D>();
        _hitbox.enabled = false; // initial state
        //_distance = Math.Abs(_hitEnd.position.x - _hitStart.position.x);
        _direction = _hitEnd.position.x >= _hitStart.position.x ? 1f : -1f;
        _speed = 1f;
        //_parentPos = gameObject.GetComponentInParent<Transform>().position;
        _activeSprite.gameObject.SetActive(false);
        //_rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        // move towards _hitEnd
        // when it reaches _hitEnd -> destroy
        if(_hitbox.enabled)
        {
            _hitbox.offset = Vector2.MoveTowards(_hitbox.offset,_hitEnd.localPosition,_speed*Time.fixedDeltaTime);
            //if(_hitbox.offset.x < _hitEnd.localPosition.x)
                //_hitbox.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("trigger - "+other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
            StartCoroutine(StartPunch());
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("collider - "+other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(_direction*_force,0), ForceMode2D.Force);
    }

    private IEnumerator StartPunch()
    {
        _activeSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _hitbox.offset = _hitStart.localPosition;
        _hitbox.enabled = true;
        yield return new WaitForSeconds(0.75f);
        _hitbox.enabled = false;
        _activeSprite.gameObject.SetActive(false);

    }
}
