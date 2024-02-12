using UnityEngine;

public class RopeSegmentInteractable : Interactable
{
    private RopeController _parentObj;
    public RopeController ParentObject{set{_parentObj = value;} private get{return _parentObj;}}

    public override void OnClick(GameObject clickingEntity)
    {
        Debug.Log("Entity Attached: " + clickingEntity);
        if(!ParentObject.HasAttachedEntity)
            ParentObject.Attach(clickingEntity, gameObject);
        else
            ParentObject.Deattach();
    }
}


