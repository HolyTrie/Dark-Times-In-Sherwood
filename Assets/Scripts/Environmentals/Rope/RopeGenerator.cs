using UnityEngine;
public class RopeGenerator : MonoBehaviour
{
    //https://www.youtube.com/watch?v=dx3jb4muLjQ&ab_channel=Brackeys
    [Tooltip("The hook the rope is connected to")]
    [SerializeField] public Rigidbody2D m_hook;

    [Tooltip("The prefabs that the rope will hold")]
    [SerializeField] public GameObject[] m_prefabRopeSegments;

    [Tooltip("How many segments the rope will have")]
    [SerializeField] public int m_numberOfSegments;
    void Start()
    {
        GenerateRope();
    }
    private void GenerateRope()
    {
        Rigidbody2D currRigidBody = m_hook;
        int index;
        GameObject newSegment;
        HingeJoint2D newHingeJoint;
        for (int i = 0; i < m_numberOfSegments; ++i)
        {
            index = Random.Range(0, m_prefabRopeSegments.Length); // randomly select a prefab to attach as the next segment
            newSegment = Instantiate(m_prefabRopeSegments[index]);
            newSegment.transform.parent = transform;
            newSegment.transform.position = transform.position;
            newHingeJoint = newSegment.GetComponent<HingeJoint2D>();
            newHingeJoint.connectedBody = currRigidBody; //add the previous body as a connection via a hingeJoint2D
            currRigidBody = newSegment.GetComponent<Rigidbody2D>(); // continue with the new segment next iteration.
        }
    }
}