using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;
public class FighterDialogTown : MonoBehaviour, IDialog
{
    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Jason";
        string playerName = "Robin Of Locksley";

        Monologue JasonMonoEnding = new(Entityname, "For now, lets enjoy the party and gather with the town folks, im sure we can find a clue to help mend your soul.");
        Monologue JasonMonoToChoice = new(Entityname, "I understand how you feel, dont worry we will never stop looking for a solution, we will steal every book in the world if neccessary!",JasonMonoEnding);

        Monologue Bravely = new(playerName, "Don't worry my friend, it is my long years at war that have dulled my sense to danger and i have paid the price for my own hubris.", JasonMonoToChoice);
        Monologue Despair = new(playerName, "I should have ran as well, i was caught in the ritual and now my soul is fractued and my life is cursed..",JasonMonoToChoice);

        Choices playerChoicetoOpening = new(Entityname, "Robin!, im so glad you survived, i'm sorry that we ran and left you behind, when we saw the sheriff we were scared for our lifes and ran like cowards",
                                            ChoiceList(
                                                Choice("Respond Bravely", Bravely),
                                                Choice("Respond With Despair", Despair)));

        return playerChoicetoOpening;
    }
}