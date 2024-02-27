using UnityEngine;

public class OnClickEventManager : MonoBehaviour
{
    private GameObject PauseMenu;

    private void Awake()
    {
        PauseMenu = GameObject.Find("PauseMenu");
    }

    private void Start()
    {
        PauseMenu.SetActive(false);
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