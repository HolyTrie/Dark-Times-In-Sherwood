using UnityEngine;

public class OnClickEventManager : MonoBehaviour
{
    private GameObject PauseMenu;

    private void Awake()
    {
        PauseMenu = GameObject.Find("PauseMenu");
    }
    
    public void PauseGame()
    {
        GameManager.PauseGame();
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        GameManager.ResumeGame();
        PauseMenu.SetActive(false);
    }
}