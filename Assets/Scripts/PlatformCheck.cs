using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    //public OneWayPlatform Curr { get { return _currPlatform; } private set { _currPlatform = value; } }
    [Header("Top platform check")]
    [SerializeField]
    private LayerMask _whatIsPlatform;
    [SerializeField,Tooltip("will be flipped symmetrically for left side")] private Transform _topRight;
    [SerializeField] private float _topRightRadius = 0.5f;
    private Vector2 _topRightOrigin;
    [Header("Bottom platform check")]
    [SerializeField] private Transform _bottomRight;
    private Vector2 _bottomRightOrigin;
    [SerializeField] private Transform _bottomLeft;
    [SerializeField] private float _bottomBoxHeight = 0.5f;
    private Vector2 _bottomLeftOrigin;
    //private OneWayPlatform _currPlatform;
    
    //[SerializeField,Tooltip("how much time in seconds the curr platform will ignore collsion when trying to pass through it")]
    //private float _ignorePlatformDuration = 0.5f;
    private PlayerController _pc;
    private void OnEnable() 
    {
        _pc = FindObjectsOfType<PlayerController>()[0]; // wont work well with more than 1 player!
    }
    private void Awake() {
        _pc = FindObjectsOfType<PlayerController>()[0]; // wont work well with more than 1 player!
    }
    private void FixedUpdate() {
        /*
        1. draw top right boxe
        2. draw bottom box
        3. store hits with platform layer only
        4. only 1
        */
        if(GameManager.PlayerSubStateType == ESP.States.LedgeGrabState)
        {
            return;
        }
        _topRightOrigin = _topRight.position;
        _bottomLeftOrigin = _bottomLeft.position;
        _bottomRightOrigin = _bottomRight.position;
        var _topOffsetX = 0.1f;

        Vector2 size = new(Mathf.Abs(_topRightOrigin.x - _pc.transform.position.x)+_topOffsetX,_topRightRadius);
        Vector2 center = new(0.5f*(_pc.transform.position.x-_topOffsetX+_topRightOrigin.x),_topRightOrigin.y);
        var TopHit = Physics2D.BoxCast(center,size,0f,Vector2.up,0f,_whatIsPlatform);

        center = new(0.5f*(_bottomLeftOrigin.x+_bottomRightOrigin.x),0.5f*(_bottomLeftOrigin.y+_bottomRightOrigin.y));
        size.x = Mathf.Abs(_bottomLeftOrigin.x-_bottomRightOrigin.x);
        size.y = 0.5f*_bottomBoxHeight;
        var BottomHit = Physics2D.BoxCast(center,size,0f,-Vector2.up,0f,_whatIsPlatform);
        if(TopHit)
        {
            //Debug.Log("Top hit = "+TopHit.transform.name);
            var oneWayPlatform = TopHit.transform.GetComponent<OneWayPlatform>();
            bool allowsGoingUp = oneWayPlatform.Type != OneWayPlatform.OneWayPlatforms.GoingDown;
            if((!GameManager.PlayerControls.JumpIsPressed || _pc.Velocity.y <=0) && allowsGoingUp)
            {
                _pc.GrabLedgeFromBelow(TopHit.collider);
            }
        }
        else if(BottomHit)
        {
            //Debug.Log("Bottom hit = "+BottomHit.transform.name);
            var oneWayPlatform = BottomHit.transform.GetComponent<OneWayPlatform>();
            bool allowsGoingDown = oneWayPlatform.Type != OneWayPlatform.OneWayPlatforms.GoingUp;
            if(GameManager.PlayerControls.DownJumpIsPressed && allowsGoingDown)
            {
                _pc.Animator.Play("crouch");
                _pc.GrabLedgeFromAbove(BottomHit.collider);
            }
        }
    }

    private void OnDrawGizmos() {
        _topRightOrigin = _topRight.position;
        _bottomLeftOrigin = _bottomLeft.position;
        _bottomRightOrigin = _bottomRight.position;
        _pc = FindObjectsOfType<PlayerController>()[0]; // wont work well with more than 1 player!
        Gizmos.color = Color.green;
        var _topOffsetX = 0.1f;
        Vector2 size = new(Mathf.Abs(_topRightOrigin.x - _pc.transform.position.x)+_topOffsetX,_topRightRadius);
        Vector2 center = new(0.5f*(_pc.transform.position.x-_topOffsetX+_topRightOrigin.x),_topRightOrigin.y);
        Gizmos.DrawWireCube(center,size);
        center = new(0.5f*(_bottomLeftOrigin.x+_bottomRightOrigin.x),0.5f*(_bottomLeftOrigin.y+_bottomRightOrigin.y));
        size.x = Mathf.Abs(_bottomLeftOrigin.x-_bottomRightOrigin.x);
        size.y = 0.5f*_bottomBoxHeight;
        Gizmos.DrawWireCube(center,size);
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(((1<<other.gameObject.layer) & _whatIsPlatform) == 0)
        {
            return;
        }
        var candidate = other.GetComponent<OneWayPlatform>();
        bool allowsGoingUp = candidate.Type != OneWayPlatform.OneWayPlatforms.GoingDown;
        bool isPlayerBelow = !IsPlayerAbovePlatform(other);
        if(isPlayerBelow && allowsGoingUp)
        {
            //_pc.PassingThroughPlatform = true;
            //Curr = candidate;
            //_pc.GrabLedgeFromBelow(Curr.Collider);
            //StartCoroutine(WaitToReapplyCollision(Curr,_ignorePlatformDuration));
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        if(((1<<other.gameObject.layer) & _whatIsPlatform) == 0)
        {
            //Debug.Log(other.gameObject.name);
            return;
        }
        Debug.Log("standing on platform");
        var candidate = other.GetComponent<OneWayPlatform>();
        bool allowsGoingDown = candidate.Type != OneWayPlatform.OneWayPlatforms.GoingUp;
        bool isPlayerAbove = IsPlayerAbovePlatform(other);
        bool downJumpPressed = _pc.GetComponent<PlayerControls>().DownJumpIsPressed;
        if(downJumpPressed && isPlayerAbove && allowsGoingDown)
        {
            Debug.Log("dropping to ledge grab from platform");
            _pc.Animator.Play("crouch");
            //_pc.PassingThroughPlatform = true;
            //Curr = candidate;
            //_pc.GrabLedgeFromAbove(Curr.Collider);

            //StartCoroutine(WaitToReapplyCollision(Curr,_ignorePlatformDuration));
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        //Curr = null;
    }
    private bool IsPlayerAbovePlatform(Collider2D other)
    {
        var collisionPoint = other.ClosestPoint(transform.position);
        var collisionNormal = (Vector2)transform.position - collisionPoint;
        if(collisionNormal.y >= 0)
            return true;
        return false;
    }
    private IEnumerator WaitToReapplyCollision(OneWayPlatform platform,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _pc.PassingThroughPlatform = false;
        //_currPlatform = null;
    }
    */
}
