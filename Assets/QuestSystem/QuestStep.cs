using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            // Adavnce quest forward after that we've finsihed this step 

            Destroy(this.gameObject);
        }
    }
}
