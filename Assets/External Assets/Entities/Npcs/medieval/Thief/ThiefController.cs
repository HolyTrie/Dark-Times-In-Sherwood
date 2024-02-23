using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;
public class ThiefController : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Will";
        string playerName = "Robin Of Locksley";

        Monologue robinMono3 = new(playerName, "Thank you, Will. I'll make sure the Sheriff pays for what he's done to our home.");
        Monologue WillMono3 = new (Entityname, "There's an old shaft beneath the well that leads directly into the mansion. It's our best chance of getting inside without alerting the guards. Once we're in, we'll need to move quickly to retrieve whatever remains of value and put an end to the Sheriff's schemes",robinMono3);

        Monologue robinMono2 = new(playerName, "What's our next move?", WillMono3);
        Monologue WillMono2 = new (Entityname, "Word has it,that the Sheriff's vaults have been emptied of gold. Instead, they're filled with dark artifacts and forbidden tomes. But that's not the worst of it. The Sheriff's meddling with dark magic has unleashed horrors upon Sherwood",robinMono2);
        
        Monologue robinOpening = new(playerName, "Tell me what you've learned, Will.", WillMono2);
        Monologue WillOpening = new(Entityname, "Robin!, You've returned just in time. The Sheriff's mansion is swarming with guards, but we've uncovered a way inside.",robinOpening);


        return WillOpening;
    }

}