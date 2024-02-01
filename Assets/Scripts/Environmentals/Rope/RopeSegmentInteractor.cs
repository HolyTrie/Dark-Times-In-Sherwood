using UnityEngine;

public class RopeSegmentInteractor : MonoBehaviour, IInteractable
{
    private RopeController _parent;
    public RopeController Parent{set{_parent = value;}}

    public void DisplayGUI()
    {
        throw new System.NotImplementedException();
    }

    public void OnClick()
    {
        //attach player
        throw new System.NotImplementedException();
    }
}
