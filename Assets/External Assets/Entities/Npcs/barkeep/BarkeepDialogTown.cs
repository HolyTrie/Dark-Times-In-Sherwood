using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Dialogue;

public class BarkeepDialogTown : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Amit";
        string playerName = "Robin Of Locksley";

        Monologue AmitMonoToChoice = new(Entityname, "Well, it's hard, but thanks to your latest victory, the folks morale upped, if there's something i've learned is that evil never lasts.");

        Monologue Shop = new(Entityname, "No problem, a price for every pocket!");
        Monologue Talk = new(playerName, "Actually, i wanted to talk, how are the folks doing these days? with the sheriff on thier necks?", AmitMonoToChoice);

        Choices playerChoicetoOpening = new(Entityname, "Welcome to my humble shop, what would you like to do today?",
                                            ChoiceList(
                                                Choice("Shop", Shop),
                                                Choice("Talk", Talk)));

        return playerChoicetoOpening;
    }
}
