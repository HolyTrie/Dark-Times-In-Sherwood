using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//auto-created singleton class - https://gist.github.com/kurtdekker/775bb97614047072f7004d6fb9ccce30
public sealed class GameManager : MonoBehaviour 
{
    // !!!!!! DO NOT PUT THIS IN ANY SCENE; this code auto-instantiates itself once.
    private static GameManager _Instance;
	public static GameManager Instance
	{
		get
		{
			if (!_Instance)
			{
				_Instance = new GameObject().AddComponent<GameManager>();
				_Instance.name = _Instance.GetType().ToString(); // name it for easy recognition
				DontDestroyOnLoad(_Instance.gameObject); // mark root as DontDestroyOnLoad();
			}
			return _Instance;
		}
	}
    private static bool _isPlayerGhosted = false;
    public static bool IsPlayerGhosted{get; set;}
}
