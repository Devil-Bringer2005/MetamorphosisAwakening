using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("----------AudioSource----------")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("----AudioClip------")]
    public AudioClip background;
    public AudioClip battleMusic;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ChangeBGM(AudioClip newClip)
    {
        musicSource.Stop();
        background = newClip;
        musicSource.clip = background;
        musicSource.Play();
    }

    
}
