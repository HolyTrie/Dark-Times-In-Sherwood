using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    [SerializeField] GameObject dialog;
    public override void OnClick(GameObject clickingEntity)
    {
        dialog.GetComponent<IDialog>().StartDialog();
    }
}
