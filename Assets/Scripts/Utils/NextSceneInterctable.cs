using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneInterctable : Interactable
{
    [SerializeField] private int _sceneIndex = -1;
    public override void OnClick(GameObject clickingEntity)
    {
        StartCoroutine(NextSceneFadeOut());
    }

    private IEnumerator NextSceneFadeOut()
    {
        Animator _blackScreen = GameObject.Find("BlackScreen").GetComponent<Animator>();
        _blackScreen.Play("BlackScreenFadeOutAnim");
        yield return new WaitForSeconds(1.1f);
        GameManager.LoadScene(_sceneIndex);
    }
}
