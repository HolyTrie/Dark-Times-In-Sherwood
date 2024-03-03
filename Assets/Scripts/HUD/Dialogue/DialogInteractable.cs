using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    [SerializeField] GameObject dialog;
    public override void OnClick(GameObject clickingEntity)
    {
        // Debug.Log("Dialog name : " + dialog.name);
        dialog.GetComponent<IDialog>().StartDialog();
    }
}
