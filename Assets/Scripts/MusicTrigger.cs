using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [Range(0,1)]
    public float volumeAmount;
    public AudioClip levelClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.ChangeBGM(levelClip);
            AudioManager.instance.musicSource.volume = volumeAmount;
        }
    }

}
