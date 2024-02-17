using UnityEngine;

/**
This is used to Flip an entity by a collider, when it reaches to its end or start position, it will flip direction.
*/
public class FlipByCollider : MonoBehaviour
{
    [Tooltip("Entity To Flip")]
    [SerializeField] EntityBoundedMovement Entity;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "EntityStartPosition")
            Entity.setDirection(true);
        if (other.tag == "EntityEndPosition")
            Entity.setDirection(false);
    }
}