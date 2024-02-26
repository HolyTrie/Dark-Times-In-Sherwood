using System.Collections;
using System.Collections.Generic;
using DTIS;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public enum OneWayPlatforms {GoingUp, GoingDown, Both}
    [SerializeField] private OneWayPlatforms _type = OneWayPlatforms.Both;
    public OneWayPlatforms Type {get{return _type;}}
    [SerializeField] private float _delay = 0.75f;
    private Collider2D _collider;
    [SerializeField] private GameObject _player;
    private PlayerController _pc;
    private PlayerControls _controls;
    private Collider2D[] _playerColliders;
    void Awake() {
        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player"); //not optimal
    }
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _playerColliders = _player.GetComponents<Collider2D>();
        _pc = Util.GetPlayerController();
        if(_pc == null)
        {
            _pc = _player.GetComponent<PlayerController>();
        }
        _controls = _player.GetComponent<PlayerControls>();
    }
    private void IgnorePlayerCollision(bool value)
    {
        foreach(var col in _playerColliders)
            Physics2D.IgnoreCollision(col,_collider,value);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject != _player && !other.gameObject.CompareTag("PlayerSensor")) 
            return;
        bool allowsGoingDown = Type != OneWayPlatforms.GoingDown;
        bool isPlayerBelow = GetContactAngle(other) < 0; //negative angle --> player is collidng form below
        if(isPlayerBelow && allowsGoingDown)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _pc.SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject != _playerSensor) 
            return;
        Debug.Log(other.gameObject.name);
        bool allowsGoingDown = Type != OneWayPlatforms.GoingDown;
        bool isPlayerBelow = GetContactAngle(other) < 0; //negative angle --> player is collidng form below
        if(isPlayerBelow && allowsGoingDown)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _pc.SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }
    */
    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject != _player && !other.gameObject.CompareTag("PlayerSensor")) 
            return;
        Debug.Log(other.gameObject.name);
        bool downwardsPressed = _controls.ActionMap.All.Down.WasPressedThisFrame();
        bool isPlayerAbove = GetContactAngle(other) > 0; //positive angle --> player is collidng form above
        bool allowsGoingDown = Type != OneWayPlatforms.GoingUp;
        if(downwardsPressed && isPlayerAbove && allowsGoingDown)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _pc.SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.CompareTag("PlayerSensor"))
            return;
        bool downwardsPressed = _controls.ActionMap.All.Down.WasPressedThisFrame();
        bool allowsGoingDown = Type != OneWayPlatforms.GoingUp;
        bool isPlayerAbove = false;
        var hit = Physics2D.Raycast(_pc.transform.position,-Vector2.up,2f,_pc.WhatIsGround);
        if(hit)
        {
            var angle = Vector2.Angle(hit.point,Vector2.up);
            if(angle > 0)
                isPlayerAbove = true;
        }
        else
        {
            return;
        }
        if(downwardsPressed && isPlayerAbove && allowsGoingDown)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _pc.SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }
    private IEnumerator WaitToReapplyCollision(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        IgnorePlayerCollision(false); //reapply collisions with player
        _player.GetComponent<PlayerController>().SetPassingThroughPlatform(false);
    }
    private float GetContactAngle(Collision2D collision)
    {
        var contactPoint = collision.GetContact(0).point;
        var dir = collision.transform.position - (Vector3)contactPoint;
        dir = collision.transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
    /*
    private float GetContactAngle(Collider2D other) // does not work as intended at all! 
    {
        ContactPoint2D[] points = new ContactPoint2D[10];
        var contactPoints = other.GetContacts(points);
        if(contactPoints == 0)
        {
            Debug.LogError("no contacts for one way trigger");
        }
        var dir = (Vector2)other.transform.position - points[0].point;
        dir = other.transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
    */
}
