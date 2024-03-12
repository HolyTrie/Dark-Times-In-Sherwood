using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Dialogue;
using DTIS;

public class SceneSherwoodTownController : MonoBehaviour
{
    void Start()
    {
        Util.GetPlayerController().Animator.Play("get-up");
        FindObjectOfType<DialogueManager>().StartDialogue(Conversation());
    }
    private DialogueSection Conversation()
    {
        string Entityname = "Shaked & Yonathan";

        Monologue CreatorsNote = new(Entityname, "Thanks For playing our demo!, enjoy walking in town, talking with the people :)");
        return CreatorsNote;
    }
}
