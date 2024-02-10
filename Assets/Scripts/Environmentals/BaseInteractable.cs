using UnityEngine;

public class BaseInteractable : Interactable
{
    /*
        This class will be used for displaying UI elemnts only, override this class and it's OnClick to add further logic!
    */
    private RopeController _parent;
    public RopeController Parent{set{_parent = value;} private get{return _parent;}}

    public override void OnClick(GameObject entity)
    {
        Debug.Log("Empty on-click");
        return; // this is a class with no logic for OnClick!.
    }
}


