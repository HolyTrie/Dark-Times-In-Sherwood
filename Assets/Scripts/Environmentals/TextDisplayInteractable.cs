using UnityEngine;

public sealed class TextDisplayInteractable : Interactable
{
    /*
        This class will be used for displaying UI elemnts only, override this class and it's OnClick to add further logic!
    */
    // TODO: make a different GUI text component.
    public override void OnClick(GameObject entity)
    {
        Debug.Log("Empty on-click");
        return; // this is a class with no logic for OnClick!.
    }
    
}


