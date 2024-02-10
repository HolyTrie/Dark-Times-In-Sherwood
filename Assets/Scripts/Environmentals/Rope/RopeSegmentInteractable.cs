using UnityEngine;

public class RopeSegmentInteractable : BaseInteractable
{
    private RopeController _parentObj;
    public RopeController ParentObject{set{_parentObj = value;} private get{return _parentObj;}}

    public override void OnClick(GameObject entity)
    {
        Debug.Log("Entity Attached: " + entity);
        if(!ParentObject.HasAttachedEntity)
            ParentObject.Attach(entity, this.gameObject);
        else
            ParentObject.Deattach();
    }
}


