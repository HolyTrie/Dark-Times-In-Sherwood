using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;

public interface IDialog
{
    public void StartDialog();
}
public class SherifController : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string localName = "Sheriff";

        Monologue fine = new(localName, "Nice to hear, have a good day");
        Monologue not_fine = new(localName, "That's too bad, hope it will be better");

        Choices b = new(localName, "How are you today?",
                                ChoiceList(
                                    Choice("Fine", fine),
                                    Choice("Not so fine...", not_fine)));

        Monologue a = new(localName, "Good Morning, I'm Sheriff", b);

        return a;
    }

}
