using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using static Dialogue;

public class DialogueOptionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] DialogueSection leadsTo;

    private DialogueManager manager;

    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
    }

    public void SetDisplay(string optionText, DialogueSection nextDialogueSection)
    {
        contentText.text = optionText;
        leadsTo = nextDialogueSection;
    }

    public void ProceedOnClick()
    {
        if(manager.displayingChoices)
        {
            return;
        }

        manager.currentSection = leadsTo;
        manager.DisplayDialogue();
    }
}
