using UnityEngine;

public class RopeController : Climbable
{
    [SerializeField] private RopeGenerator _generatorScript;

    private HingeJoint2D playerHJ;
    void Start()
    {
        if (_generatorScript == null)
        {
            _generatorScript = GetComponent<RopeGenerator>();
        }

    }
    protected override void OnAttach(GameObject RopeSegment)
    {
        playerHJ = AttachedEntity.transform.GetComponent<HingeJoint2D>();
        Debug.Log("THIS SEGMENT IS: " + RopeSegment.transform.parent);
        playerHJ.connectedBody = RopeSegment.GetComponent<Rigidbody2D>();
        playerHJ.enabled = true;
        AttachedEntity.transform.parent.parent = RopeSegment.transform.parent;
        Debug.Log("On Attach");
    }
    protected override void OnDeattach()
    {
        playerHJ = AttachedEntity.transform.GetComponent<HingeJoint2D>();
        playerHJ.connectedBody = null;
        playerHJ.enabled = false;
        AttachedEntity.transform.parent.parent = null;
        Debug.Log("On Deattach");
    }
}
