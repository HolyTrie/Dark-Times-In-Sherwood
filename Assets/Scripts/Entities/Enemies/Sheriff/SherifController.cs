using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static Dialogue;
public class SherifController : MonoBehaviour, IDialog
{
    private string playerChocie;

    public void StartDialog()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Sheriff";
        string playerName = "Robin Of Locksley";

        // choice B //
        Monologue SheriffUltimatum = new(Entityname, "Or perhaps,you need a reminder of your place in this world. You see, Locksley, I have grown tired of your insolence. If you insist on defying me, then I will have no choice but to resort to more... drastic measures.");


        // choice A //
        Monologue robinReplyToProposition = new(playerName, "I will never sell my soul to the likes of you, Sheriff. My allegiance lies with the people of Sherwood, and I will stop at nothing to see justice served.");
        Monologue SheriffProposition = new(Entityname, "But perhaps,we can come to an arrangement. You see, I have a proposition for you, Locksley. Join me, pledge your allegiance to my cause, and together we can rule over Sherwood as kings. Imagine the power we could wield, the riches we could amass. It's a tempting offer, wouldn't you agree?", robinReplyToProposition);

        Choices playerChoicetoOpening = new(Entityname, "I can see that your friends left you here, you should consider your next moves carefully..",
                                            ChoiceList(
                                                Choice("The Sheriff's Proposition", SheriffProposition),
                                                Choice("The Sheriff's Ultimatum", SheriffUltimatum)));

        Monologue sheriffOpening = new(Entityname, "Ah, Robin of Locksley. How predictable. It seems the years have not dulled your sense of self-righteousness. But you are mistaken if you think you can waltz back into Sherwood and challenge my authority", playerChoicetoOpening);


        Monologue robinOpening = new(playerName, "Sheriff, I have returned to reclaim what is rightfully mine. Your tyrannical rule ends here and now!", sheriffOpening);

        return robinOpening;
    }

    public void Update()
    {
        DialogChoices();
    }
    private void DialogChoices()
    {
        if(GameManager.playerChoices!=null)
            Debug.Log(GameManager.playerChoices); //testting
    }

}

// Monologue charge = new(Entityname, "Brave move, now you will face death."); 
// Monologue leave = new(Entityname, "Smart choice, now go back to where you came from.");

// Choices b = new(Entityname, "Seems like your friends left you alone, your next steps are going to decide your fate...",
//                         ChoiceList(
//                             Choice("You will PAY!", charge),
//                             Choice("Go back", leave)));

// Monologue a = new(Entityname, "I've awaited your arrival", b);