using UnityEngine;
using DTIS;
public class OnClickEventManager : MonoBehaviour
{
    private GameObject PauseMenu;
    private GameObject MainMenu;
    private GameObject StarterPauseMenu;
    private GameObject KeyBindings;
    private const int StartScene = 1;
    private const int GameStartScene = 2;

    private void Awake()
    {
        PauseMenu = GameObject.Find("PauseMenu");
        MainMenu = GameObject.Find("MainMenu");
        StarterPauseMenu = GameObject.Find("StarterMenu");
        KeyBindings = GameObject.Find("KeyBindings");
    }

    private void Start()
    {
        if (StarterPauseMenu != null)
            StarterPauseMenu.SetActive(false);
        if (KeyBindings != null)
            KeyBindings.SetActive(false);
    }

    public void PauseGame()
    {
        GameManager.PauseGame();
        PauseMenu.gameObject.SetActive(true);
        StarterPauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        GameManager.ResumeGame();
        PauseMenu.gameObject.SetActive(false);
        StarterPauseMenu.SetActive(false);
    }

    public void StartGame()
    {
        GameManager.LoadScene(StartScene);
    }

    public void SkipCutsecene()
    {
        GameManager.LoadScene(GameStartScene);
    }

    public void HelpMenu()
    {
        StarterPauseMenu.SetActive(false);
        KeyBindings.SetActive(true);
    }

    public void HideHelpMenu()
    {
        StarterPauseMenu.SetActive(true);
        KeyBindings.SetActive(false);
    }
}