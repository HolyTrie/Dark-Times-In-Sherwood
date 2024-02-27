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
    private float _ignorePlatformDuration = 0.25f;
    private PlayerController _pc;
    private Collider2D[] _playerColliders;
    //private Collider2D _collider;

    void Start()
    {
        //_collider = GetComponent<Collider2D>();
        _pc = FindObjectsOfType<PlayerController>()[0]; // wont work well with more than 1 player!
        _playerColliders = _pc.GetComponents<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Platform"))
            return;
        
        Curr = other.GetComponent<OneWayPlatform>();
        bool allowsGoingUp = Curr.Type != OneWayPlatform.OneWayPlatforms.GoingDown;
        bool isPlayerBelow = !IsPlayerAbovePlatform(other);
        if(isPlayerBelow && allowsGoingUp)
        {
            _pc.PassingThroughPlatform = true;
            StartCoroutine(WaitToReapplyCollision(_ignorePlatformDuration));
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Platform"))
            return;
        Curr = other.GetComponent<OneWayPlatform>();
        bool allowsGoingDown = Curr.Type != OneWayPlatform.OneWayPlatforms.GoingUp;
        bool isPlayerAbove = IsPlayerAbovePlatform(other);
        bool jumpPressed = _pc.GetComponent<PlayerControls>().ActionMap.All.Down.WasPerformedThisFrame();
        Debug.Log($"jump pressed = {jumpPressed} | enter allows up ={allowsGoingDown} and isPlayerAbove = {isPlayerAbove}");
        if(isPlayerAbove && allowsGoingDown)
        {
            Debug.Log("stayed and passing through");
            _pc.PassingThroughPlatform = true;
            StartCoroutine(WaitToReapplyCollision(_ignorePlatformDuration));
        }
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
        Debug.Log("released passing through");
        _pc.PassingThroughPlatform = false;
        _currPlatform = null;
    }
}
