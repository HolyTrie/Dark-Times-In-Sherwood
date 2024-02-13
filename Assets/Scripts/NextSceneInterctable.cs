using UnityEngine;

public class NextSceneInterctable : Interactable
{
    [SerializeField] private int _sceneIndex = -1;
    public override void OnClick(GameObject clickingEntity)
    {
        if (_sceneIndex >= 0)
        {
                GameManager.LoadScene((int)_sceneIndex);
        }
        else
            GameManager.NextScene();
    }
}
