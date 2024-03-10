using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneCollider : MonoBehaviour
{
    [SerializeField] private int _sceneIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
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
