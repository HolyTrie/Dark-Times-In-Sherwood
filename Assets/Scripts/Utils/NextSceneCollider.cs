using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneCollider : MonoBehaviour
{
    [SerializeField] private int _sceneIndex = -1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_sceneIndex >= 0)
        {
            GameManager.LoadScene(_sceneIndex);
        }
        else
            GameManager.NextScene();
    }
}
