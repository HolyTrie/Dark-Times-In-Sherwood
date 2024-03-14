using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DTIS;
public class ArrowTrapSpawner : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] int DMG;
    [SerializeField] LayerMask Ground;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        //add velocity
        _rigidbody2D.velocity = new Vector2(transform.position.x, transform.position.y).normalized * force;
        float angle = Mathf.Atan2(transform.rotation.y, transform.rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // appllying the caluclated angle.
    }
    /*if arrow hits floor it will destroy on impact (maybe do this better, but for now will suffice)*/
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") // Damage to Player //other.gameObject.layer == 1 << 10
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.HpBar.depleteHp(DMG);
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == 7) // ground layer
            Destroy(this.gameObject);
    }
}

