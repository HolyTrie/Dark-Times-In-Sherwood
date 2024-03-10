using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;
public class ThiefDialogTown : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Will";
        string playerName = "Robin Of Locksley";

        Monologue WillMono3 = new (Entityname, "You too Robin.");

        Monologue robinMono2 = new(playerName, "Alright, have fun will, talk laters.", WillMono3);
        Monologue WillMono2 = new (Entityname, "Yes, indeed. we shall regroup later after the party",robinMono2);
        
        Monologue robinOpening = new(playerName, "That's the least i could do, we've long road ahead of us", WillMono2);
        Monologue WillOpening = new(Entityname, "Robin!, glad you came back alive, the town's folk morale has upped thanks to you",robinOpening);

        return WillOpening;
    }

}