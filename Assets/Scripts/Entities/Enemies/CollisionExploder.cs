using System.Collections;
using UnityEngine;
/**
 * This component triggers an explosion effect and destroys its object 
 * whenever its object collides with something in a velocity above the threshold.
 */
[RequireComponent(typeof(Rigidbody2D))]
public class CollisionExploder : MonoBehaviour
{
    [SerializeField] float minImpulseForExplosion = 1.0f;
    [SerializeField] GameObject explosionEffect = null;
    [SerializeField] float explosionEffectTime = 0.68f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // In 3D, the Collision object contains an .impulse field.
        // In 2D, the Collision2D object does not contain it - so we have to compute it.
        // Impulse = F * DeltaT = m * a * DeltaT = m * DeltaV
        if (collision.collider.tag == "Arrow")
        {
            Debug.Log("ARROW FALSE TRIGGER");
            collision.collider.isTrigger = false; // after hitting the player, set the arrow to trigger so it can be destroyed once reached ground.
        }
        float impulse = collision.relativeVelocity.magnitude * rb.mass;
        //Debug.Log(gameObject.name + " collides with " + collision.collider.name + " at velocity " + collision.relativeVelocity + " [m/s], impulse " + impulse + " [kg*m/s]");
        if (impulse > minImpulseForExplosion)
        {
            // StartCoroutine(Explosion());
            Destroy(rb.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Arrow")
        {
            Debug.Log("ARROW TRUE TRIGGER");
            collision.collider.isTrigger = true; // after hitting the player, set the arrow to trigger so it can be destroyed once reached ground.
        }
    }

    IEnumerator Explosion()
    {
        explosionEffect.SetActive(true);
        yield return new WaitForSeconds(explosionEffectTime);
        Destroy(this.gameObject);
    }
}