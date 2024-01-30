using UnityEngine;

public class FlipByCollider : MonoBehaviour
{
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