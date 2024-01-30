using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    // https://www.youtube.com/watch?v=yQiR2-0sbNw&ab_channel=juul1a - god bless.
    public GameObject connectedAbove,connectedBelow;
    void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();
        if(aboveSegment != null) // iterative assignment of neighbours (bottom --> top) 
        {
            aboveSegment.connectedBelow = gameObject; //tells the upstairs neighbour they live right below them and can hear everything...
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y; // tells us the offset we need to reach the bootom of the above sprite
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0f,spriteBottom*=-1); //  hence the "*=-1" in this line
        }
        else //top and final segment
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0f,0f);
        }
    }
}
