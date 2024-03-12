using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBossManager : MonoBehaviour
{
    public void OnClickLoadSherwoodTown()
    {
        StartCoroutine(AfterBossDeath());
    }

    private IEnumerator AfterBossDeath()
    {
        Animator _blackScreen = GameObject.Find("BlackScreen").GetComponent<Animator>();
        _blackScreen.Play("BlackScreenFadeOutAnim");
        yield return new WaitForSeconds(1.1f);
        GameManager.LoadScene(6); // town
    }
}
