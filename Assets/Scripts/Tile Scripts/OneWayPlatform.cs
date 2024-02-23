using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public enum OneWayPlatforms {GoingUp, GoingDown, Both}
    [SerializeField] private OneWayPlatforms _type = OneWayPlatforms.Both;
    public OneWayPlatforms Type {get{return _type;}}
    [SerializeField] private float _delay = 0.75f;
    private Collider2D _collider;
    [SerializeField] private GameObject _player;
    [Tooltip("optional game object attached ahead of player for detection")]
    [SerializeField] private GameObject _playerSensor;
    private Collider2D[] _playerColliders;
    void Awake() {
        if(_player == null)
            _player = GameObject.FindGameObjectWithTag("Player"); //not optimal
    }
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _playerColliders = _player.GetComponents<Collider2D>();
    }
    private void IgnorePlayerCollision(bool value)
    {
        foreach(var col in _playerColliders)
            Physics2D.IgnoreCollision(col,_collider,value);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject != _player && other.gameObject != _playerSensor) 
            return;
        bool allowsGoingDown = Type != OneWayPlatforms.GoingDown;
        bool isPlayerBelow = GetContactAngle(other) < 0; //negative angle --> player is collidng form below
        if(isPlayerBelow && allowsGoingDown)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _player.GetComponent<PlayerController>().SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }
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
            _player.GetComponent<PlayerController>().SetPassingThroughPlatform(true);
            StartCoroutine(WaitToReapplyCollision(_delay));
        }
    }
    private void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject != _player && other.gameObject != _playerSensor) 
            return;
        bool downwardsPressed = _player.GetComponent<PlayerControls>().ActionMap.All.Down.WasPerformedThisFrame();
        bool isPlayerAbove = GetContactAngle(other) > 0; //positive angle --> player is collidng form above
        bool allowsGoingUp = Type != OneWayPlatforms.GoingUp;
        if(downwardsPressed && isPlayerAbove && allowsGoingUp)
        {
            IgnorePlayerCollision(true); // ignore player colliders
            _player.GetComponent<PlayerController>().SetPassingThroughPlatform(true);
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
    private float GetContactAngle(Collider2D other)
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
}
