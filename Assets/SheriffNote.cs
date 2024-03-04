using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Dialogue;
public class SheriffNote : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Sheriff";

        Monologue SherifNote = new(Entityname, "Ah poor robin, your bad choices keep coming in waves, now that i have enslaved you, i won't set you free unless you return all the artifact i lost. you might feel like your body is chaning due to the explosion ");
        return SherifNote;
    }
}
