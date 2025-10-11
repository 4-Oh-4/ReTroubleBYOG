using System;
using UnityEngine;

public class AudioManager_A : MonoBehaviour
{

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip backgroundMusic;
    public AudioClip run;
    public AudioClip buttonClick;
    public AudioClip shoot;
    public AudioClip bounce;
    public AudioClip damage;
    public AudioClip bubbleFusion;
    public AudioClip powerUP;
    public AudioClip bubbleDestroy;
    public AudioClip bubbleFission;

    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
/*
AudioManager_A audioManager;

private void Awake()
{
audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_A>();
}

audioManager.PlaySFX(audioManager.(Clip)...);

*/