using UnityEngine;
using DTIS;
public class OnClickEventManager : MonoBehaviour
{
    private GameObject PauseMenu;
    private const int StartScene = 1;

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

    public void StartGame()
    {
        GameManager.LoadScene(StartScene); 
    }
}