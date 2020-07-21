using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get => _instance; }
    public AudioClip buttonClickSound, placeBuildingSound, removebuildingSound;
    public AudioSource effectAudioSource;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayButtonClickedSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = buttonClickSound;
        effectAudioSource.Play();
    }

    internal void PlayRemoveSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = removebuildingSound;
        effectAudioSource.Play();
    }

    internal void PlayPlaceBuildingSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = placeBuildingSound;
        effectAudioSource.Play();
    }
}
