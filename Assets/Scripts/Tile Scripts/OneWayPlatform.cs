using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public enum OneWayPlatforms {GoingUp, GoingDown, Both}
    public OneWayPlatforms Type => _type;
    public Collider2D Collider => _collider;

    [SerializeField] private OneWayPlatforms _type = OneWayPlatforms.Both;
    private Collider2D _collider;
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    /*
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject != _player && !other.gameObject.CompareTag("PlayerSensor")) 
            return;
        //Debug.Log("on collision enter = "+other.gameObject.name);
        bool allowsGoingDown = Type != OneWayPlatforms.GoingDown;
        bool isPlayerBelow = GetContactAngle(other) < 0; //negative angle --> player is collidng form below
        if(isPlayerBelow && allowsGoingDown)
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
        Debug.Log("trigger stay with " + other.gameObject.name);
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
    */
}
