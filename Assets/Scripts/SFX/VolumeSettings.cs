using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class VolumeSettings : MonoBehaviour
{
    public static VolumeSettings instance;

    private static float _sfx = 1f;
    public static float SfxMultiplier { get { return _sfx; } set { _sfx = value; } }

    private static float _bgm = 1f;
    public static float BGMultiplier { get { return _bgm; } set { _bgm = value; } }

    public static VolumeSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = new GameObject().AddComponent<VolumeSettings>();
                instance.name = instance.GetType().ToString(); // name it for easy recognition
                DontDestroyOnLoad(instance.gameObject); // mark root as DontDestroyOnLoad();
            }
            return instance;
        }
    }
}
