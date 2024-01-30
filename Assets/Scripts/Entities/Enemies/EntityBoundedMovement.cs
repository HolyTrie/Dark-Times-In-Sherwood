using UnityEngine;

/**
 *  This component moves its object in a fixed speed back and forth between two points in space.
 TODO -> seperate into two - moving enemy + moving platform.
 */
public class EntityBoundedMovement : MonoBehaviour
{
    [Tooltip("The points between which the platform moves")]
    [SerializeField] Transform startPoint = null, endPoint = null;
    [SerializeField] float speed = 1f;
    bool moveFromStartToEnd = true;

    private void Start()
    {
        transform.position = startPoint.position;
    }

    void Update()
    {
        Flip();
    }

    void FixedUpdate()
    {
        // If Update is used, the player does not move with the platform.
        float deltaX = speed * Time.fixedDeltaTime;
        if (moveFromStartToEnd)
        {
            //Debug.Log("Moves To End");
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, deltaX);
        }
        else
        {  // move from end to start
            //Debug.Log("Moves To Start");
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, deltaX);
        }
    }

    public void setDirection(bool Direction)
    {
        this.moveFromStartToEnd = Direction;
    }

    private void Flip()
    {
        if (moveFromStartToEnd)
            transform.GetComponent<SpriteRenderer>().flipX = false;
        else
            transform.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.GetComponent<KeyboardMover>()) {
        //     other.transform.parent = this.transform;
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if (other.gameObject.GetComponent<KeyboardMover>()) {
        //     other.transform.parent = null;
        // }
    }
}