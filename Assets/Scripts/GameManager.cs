using System;
using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;
using UnityEngine.SceneManagement;
//auto-created singleton class - https://gist.github.com/kurtdekker/775bb97614047072f7004d6fb9ccce30
public sealed class GameManager : MonoBehaviour
{
	// !!!!!! DO NOT PUT THIS IN ANY SCENE; this code auto-instantiates itself once.
	private static GameManager _Instance;
	private static int currSceneIndex;
	private static PlayerStateMachine fsm;
	public static GameManager Instance
	{
		get
		{
			if (!_Instance)
			{
				_Instance = new GameObject().AddComponent<GameManager>();
				_Instance.name = _Instance.GetType().ToString(); // name it for easy recognition
				currSceneIndex = SceneManager.GetActiveScene().buildIndex; //TODO: currently bugs out and is always 0 !!!!
				DontDestroyOnLoad(_Instance.gameObject); // mark root as DontDestroyOnLoad();
			}
			return _Instance;
		}
	}
	private static bool _isPlayerGhosted = false;
	public static bool IsPlayerGhosted
	{
		get
		{
			return _isPlayerGhosted;
		}
		set
		{
			_isPlayerGhosted = value;
			//Debug.Log(string.Format("player ghosted = {0}",_isPlayerGhosted));
		}
	}

	private static int _money;
	public static int Money { get { return _money; } set { _money = value; } }

	public static void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void LoadScene(int index)
	{
		IsPlayerGhosted = false; // reset ghost, todo much later - get this info from scene object
		SceneManager.LoadScene(index);
		Instance.StartCoroutine(DisableEnableControls());
	}

	private static IEnumerator DisableEnableControls()
	{
		yield return null; // wait 1 frame.
		fsm.Controls.enabled = false;
		fsm.Controls.enabled = true;
	}

	public static void NextScene(int offset = 0)
	{
		//Debug.Log(String.Format("Curr Index = {0}, SceneManager.sceneCount = {1}",currSceneIndex,SceneManager.sceneCountInBuildSettings));
		currSceneIndex = (currSceneIndex + 1 + offset) % SceneManager.sceneCountInBuildSettings;
		//Debug.Log(String.Format("Curr Index = {0}, SceneManager.sceneCount = {1}",currSceneIndex,SceneManager.sceneCountInBuildSettings));
		LoadScene(currSceneIndex);
	}

	internal static void SetFSM(PlayerStateMachine playerStateMachine)
	{
		fsm = playerStateMachine;
	}

	public static void PauseGame()
	{
		Time.timeScale = 0;
		fsm.Controls.enabled = false;
	}

	public static void ResumeGame()
	{
		Time.timeScale = 1;
		fsm.Controls.enabled = true;
	}
}
