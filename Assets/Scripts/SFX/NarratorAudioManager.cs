using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] clips;

    private int currentClipIndex = 0;

    void Start()
    {
        PlayNextClip(); // first clip
    }
    void Update()
    {
        if (!Source.isPlaying)
        {
            currentClipIndex++;
            // If all clips have been played, stop the coroutine
            if (currentClipIndex >= clips.Length)
            {
                return;
            }
            PlayNextClip();
        }
    }
    void PlayNextClip()
    {
        // Make sure the currentClipIndex is within bounds
        if (currentClipIndex >= 0 && currentClipIndex < clips.Length)
        {
            Source.clip = clips[currentClipIndex];
            Source.Play();
        }
    }
}
