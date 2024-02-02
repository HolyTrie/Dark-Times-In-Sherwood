using UnityEngine;

public class RopeSegmentInteractable : Interactable
{
    private RopeController _parent;
    public RopeController Parent{set{_parent = value;} private get{return _parent;}}

    public override void OnClick(GameObject entity)
    {
        if(Parent.HasAttachedEntity)
            Parent.Deattach();
        else
            Parent.Attach(entity);
    }
}


