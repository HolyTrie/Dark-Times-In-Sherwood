using UnityEngine;
using DTIS;
public class OnClickEventManager : MonoBehaviour
{
    private GameObject PauseMenu;
    private GameObject MainMenu;
    private GameObject StarterPauseMenu;
    private GameObject KeyBindings;
    private GameObject OptionsPanel;
    private GameObject SceneSelection;
    private const int StartScene = 1;
    private const int GameStartScene = 2;

    private void Awake()
    {
        MainMenu = GameObject.Find("MainMenu");
        StarterPauseMenu = GameObject.Find("StarterMenu");
        KeyBindings = GameObject.Find("KeyBindings");
        OptionsPanel = GameObject.Find("OptionsPanel");
        SceneSelection = GameObject.Find("SceneSelection");
    }

    private void Start()
    {
        if (StarterPauseMenu != null)
            StarterPauseMenu.SetActive(false);
        if (KeyBindings != null)
            KeyBindings.SetActive(false);
        if (OptionsPanel != null)
            OptionsPanel.SetActive(false);
        if(SceneSelection != null)
            SceneSelection.SetActive(false);
    }

    public void PauseGame()
    {
        GameManager.PauseGame();
        StarterPauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        GameManager.ResumeGame();
        StarterPauseMenu.SetActive(false);
        OptionsPanel.SetActive(false);
        KeyBindings.SetActive(false);
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
        if(OptionsPanel!=null)
            OptionsPanel.SetActive(false);
            
        KeyBindings.SetActive(true);
    }

    public void HideHelpMenu()
    {
        StarterPauseMenu.SetActive(true);
        KeyBindings.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        GameManager.LoadScene(0);
        GameManager.ResetPlayer();
    }

    public void OpenVolumeSettings()
    {
        if(SceneSelection!=null)
            CloseSceneSelectionMenu();

        OptionsPanel.SetActive(true);
    }

    public void HideOptionsSettings()
    {
        OptionsPanel.SetActive(false);
    }

    public void OpenSceneSelectionMenu()
    {
        HideOptionsSettings();
        SceneSelection.SetActive(true);
    }

    public void CloseSceneSelectionMenu()
    {
        SceneSelection.SetActive(false);
    }

    public void LoadScene(int Scene)
    {
        GameManager.LoadScene(Scene);
    }


}