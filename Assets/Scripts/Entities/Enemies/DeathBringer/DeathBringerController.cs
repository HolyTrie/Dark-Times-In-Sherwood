using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using static Dialogue;

namespace DTIS
{
    public class DeatBringerController : EntityController
    {
        [Header("Boss Death")]
        [SerializeField] DialogueManager DialogUponDeath;

        private const float DeathDelaySeconds = 1.75f;

        public override void DropItems()
        {
            StartCoroutine(WaitForDeath(DeathDelaySeconds));
        }

        private IEnumerator WaitForDeath(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            DialogUponDeath.StartDialogue(Conversation());
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }

        private DialogueSection Conversation()
        {
            string Entityname = "Robin";

            Monologue SherifNote = new(Entityname, "Finally i've bested this bastard, time to return to town, to check with the folks...");
            return SherifNote;
        }
    }
}