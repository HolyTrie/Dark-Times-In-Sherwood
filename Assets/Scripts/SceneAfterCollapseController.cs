using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Dialogue;
using DTIS;
public class SceneAfterCollapseController : MonoBehaviour
{
    void Start()
    {
        Util.GetPlayerController().Animator.Play("get-up");
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Sheriff";
        string text = "Poor stupid Robin, as recompense for your meddling I have taken a part of your very being... \nA fragment of your soul is now mine to do with as I please, and to your detriment you will find that you can now interact with the realm of spirits. \n(pressing G activates a ghost state that drains sanity until death) ";
        Monologue SherifNote = new(Entityname, text);
        return SherifNote;
    }
}
