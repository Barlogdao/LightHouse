using System;
using UnityEngine;

public class AudioManager : SingletonPersistent<AudioManager>
{
    //[SerializeField]
    //EventFloatSO volumeEvent;
    public static event Action GeneralVolumeChanged;

    [SerializeField] private SoundManager soundManager;
    public SoundManager SoundManager => soundManager;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip _music, _oceanl, _battleMusic;

    public void PlayMusic()
    {
        audioSource.clip = _music;
        audioSource.Play();
    }
    public void PlayOcean()
    {
        audioSource.clip = _oceanl;
        audioSource.Play();
    }
    public void PlayBattleMusic()
    {
        audioSource.clip = _battleMusic;
        audioSource.Play();
    }

  


}
