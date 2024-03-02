using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformCheck : MonoBehaviour
{
    private OneWayPlatform _currPlatform;
    public OneWayPlatform Curr { get { return _currPlatform; } private set { _currPlatform = value; } }
    
    [SerializeField,Tooltip("how much time in seconds the curr platform will ignore collsion when trying to pass through it")]
    private float _ignorePlatformDuration = 0.5f;
    private PlayerController _pc;

    void Start()
    {
        _pc = FindObjectsOfType<PlayerController>()[0]; // wont work well with more than 1 player!
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Platform"))
            return;
        
        var candidate = other.GetComponent<OneWayPlatform>();
        bool allowsGoingUp = candidate.Type != OneWayPlatform.OneWayPlatforms.GoingDown;
        bool isPlayerBelow = !IsPlayerAbovePlatform(other);
        if(isPlayerBelow && allowsGoingUp)
        {
            _pc.PassingThroughPlatform = true;
            Curr = candidate;
            StartCoroutine(WaitToReapplyCollision(_ignorePlatformDuration));
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Platform"))
            return;
        var candidate = other.GetComponent<OneWayPlatform>();
        bool allowsGoingDown = candidate.Type != OneWayPlatform.OneWayPlatforms.GoingUp;
        bool isPlayerAbove = IsPlayerAbovePlatform(other);
        bool downJumpPressed = _pc.GetComponent<PlayerControls>().DownJumpIsPressed;
        if(downJumpPressed && isPlayerAbove && allowsGoingDown)
        {
            _pc.Animator.Play("crouch");
            _pc.PassingThroughPlatform = true;
            Curr = candidate;
            StartCoroutine(WaitToReapplyCollision(_ignorePlatformDuration));
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(Curr != null)
            if(other == Curr.Collider)
                Curr = null;
    }
    private bool IsPlayerAbovePlatform(Collider2D other)
    {
        var collisionPoint = other.ClosestPoint(transform.position);
        var collisionNormal = (Vector2)transform.position - collisionPoint;
        if(collisionNormal.y >= 0)
            return true;
        return false;
    }
    private IEnumerator WaitToReapplyCollision(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _pc.PassingThroughPlatform = false;
        _currPlatform = null;
    }
}
