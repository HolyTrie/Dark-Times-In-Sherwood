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
	private static PlayerStateMachine _fsm;
	private static PlayerController _playerController;
	public static bool PlayerIsFacingRight { get { return _playerController.FacingRight; } }
	public static string playerChoices;
	private static bool _canGhost;
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
			if(_canGhost)
			{
				_isPlayerGhosted = value;
			}
			//Debug.Log(string.Format("player ghosted = {0}",_isPlayerGhosted));
		}
	}

	private static int _money;
	public static int Money { get { return _money; } set { _money = value; } }
    public static PlayerControls PlayerControls { get { return _fsm.Controls;}}
	public static ESP.States PlayerStateType { get { return _fsm.State.Type; } }
    public static ESP.States PlayerSubStateType { get { return _fsm.SubState.Type; } }

    public static void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1f;
	}

	public static void LoadScene(int index)
	{
		IsPlayerGhosted = false; // reset ghost, todo much later - get this info from scene object
		SceneManager.LoadScene(index);
		Instance.StartCoroutine(DisableEnableControls());
		Time.timeScale = 1f;
	}

	private static IEnumerator DisableEnableControls()
	{
		yield return null; // wait 1 frame.
		if (_fsm != null)
		{
			_fsm.Controls.enabled = false;
			_fsm.Controls.enabled = true;
		}
	}
    public static void BlockGhost()
    {
        _canGhost = false;
    }
	public static void UnblockGhost()
    {
        _canGhost = true;
    }
	public static void NextScene(int offset = 0)
	{
		//Debug.Log(String.Format("Curr Index = {0}, SceneManager.sceneCount = {1}",currSceneIndex,SceneManager.sceneCountInBuildSettings));
		currSceneIndex = (currSceneIndex + 1 + offset) % SceneManager.sceneCountInBuildSettings;
		//Debug.Log(String.Format("Curr Index = {0}, SceneManager.sceneCount = {1}",currSceneIndex,SceneManager.sceneCountInBuildSettings));
		UnblockGhost(); // reset ghost controls on scene exit just in case
		LoadScene(currSceneIndex);
	}

	internal static void SetFSM(PlayerStateMachine playerStateMachine)
	{
		_fsm = playerStateMachine;
	}

	internal static void SetController(PlayerController playerController)
	{
		_playerController = playerController;
	}

	public static void PauseGame()
	{
		Time.timeScale = 0;
		StopControls();
	}

	public static void ResumeGame()
	{
		Time.timeScale = 1;
		ResumeControls();
	}

	public static void StopControls()
	{
		_fsm.Controller.Animator.Play("idle");
		_fsm.Controller.enabled = false;
		_fsm.Controls.enabled = false;
	}

	public static void ResumeControls()
	{
		_fsm.Controller.enabled = true;
		_fsm.Controls.enabled = true;
	}
}
