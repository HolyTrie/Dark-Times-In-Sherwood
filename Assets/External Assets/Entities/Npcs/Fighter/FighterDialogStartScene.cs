using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;
public class FighterDialogStartScene : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Jason";
        string playerName = "Robin Of Locksley";

        Monologue robinMono3 = new(playerName, "Thank you for the warning, I will not forget your counsel as we venture into the depths of the vaults");
        Monologue JasonMono3 = new(Entityname, "Arrows dipped in holy water may slow them down, but it will take more than mere steel to banish them back to the darkness from whence they came. We must use traps, cunning, anything at our disposal to outwit these fiends and emerge victorious", robinMono3);

        Monologue robinMono2 = new(playerName, "How do we defeat them?", JasonMono3);
        Monologue JasonMono2 = new(Entityname, "Demons, Robin. Twisted abominations born from the depths of the underworld. They hunger for the souls of the living, and they will not hesitate to tear you limb from limb if given the chance.", robinMono2);

        Monologue robinOpening = new(playerName, "What kind of creatures are we dealing with?", JasonMono2);
        Monologue JasonOpening = new(Entityname, "Robin, you must tread carefully within the Sheriff's vaults. The creatures unleashed by his dark magic are unlike anything we've faced before.", robinOpening);


        return JasonOpening;
    }
}