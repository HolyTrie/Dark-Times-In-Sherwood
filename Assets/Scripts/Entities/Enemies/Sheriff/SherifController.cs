using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Dialogue;
public class SherifController : MonoBehaviour
{

    private void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }


    private DialogueSection Conversation()
    {
        string localName = "Sheriff";

        Monologue fine = new Monologue(localName, "Nice to hear, have a good day");
        Monologue not_fine = new Monologue(localName, "That's too bad, hope it will be better");

        Choices b = new Choices(localName, "How are you today?",
                                ChoiceList(
                                    Choice("Fine", fine),
                                    Choice("Not so fine...", not_fine)));
        
        Monologue a = new Monologue(localName, "Good Morning, I'm Sheriff", b);

        return a;
    }

}
