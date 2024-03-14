using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource ButtonSFX;

    [Header("Audio Clip")]
    [SerializeField] AudioClip background;
    [SerializeField] float BackgroundVolume = 0.05f;
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] float SFXVolume = 0.25f;

    private float _orignalBGM;
    private float _originalSFX;

    void Start()
    {
        MusicSource.clip = background;
        MusicSource.volume = BackgroundVolume;
        MusicSource.Play();

        ButtonSFX.clip = buttonSFX;
        ButtonSFX.volume = SFXVolume;

        _orignalBGM = MusicSource.volume;
        _originalSFX = ButtonSFX.volume;
    }

    void Update()
    {
        BackgroundVolume = _orignalBGM * VolumeSettings.BGMultiplier;
        MusicSource.volume = BackgroundVolume;

        SFXVolume = _originalSFX * VolumeSettings.SfxMultiplier;
        ButtonSFX.volume = SFXVolume;
    }

    public void PlayButtonSFX()
    {
        ButtonSFX.Play();
    }

}
