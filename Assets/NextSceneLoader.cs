using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneLoader : MonoBehaviour
{
    [SerializeField] private int _sceneId;
    void OnEnable()
    {
        GameManager.LoadScene(_sceneId);
    }
}
