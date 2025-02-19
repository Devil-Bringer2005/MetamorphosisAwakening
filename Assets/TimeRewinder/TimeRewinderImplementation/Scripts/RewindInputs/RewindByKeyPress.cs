using System.Collections;
using UnityEngine;

/// <summary>
///  Example how to rewind time with key press
/// </summary>
public class RewindByKeyPress : MonoBehaviour
{
    [SerializeField] float rewindIntensity = 0.02f;
    [SerializeField] AudioSource rewindSound;
    [SerializeField] KeyCode keyToRewind = KeyCode.Space;
    float rewindValue = 0;
    bool isRewinding = false;
    bool isActive = false;
    float timer;

    private void Update()
    {
        if (Input.GetKeyDown(keyToRewind) && AbilityHandler.instance.CanUseRewind() && !isActive)
        {
            isActive = true;
            timer = RewindManager.Instance.HowManySecondsToTrack - 0.2f;
        }
    }

    void FixedUpdate()
    {
        if(!isActive)
        {
            if (isRewinding)
            {
                StopRewind();
            }
        }

        if (isActive)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                isActive = false;

            Rewind();
        }
    }

    private void StopRewind()
    {
        RewindManager.Instance.StopRewindTimeBySeconds();
        rewindSound.Stop();
        rewindValue = 0;
        isRewinding = false;
    }

    private void Rewind()
    {
        rewindValue += rewindIntensity;                 //While holding the button, we will gradually rewind more and more time into the past

        if (!isRewinding)
        {
            RewindManager.Instance.StartRewindTimeBySeconds(rewindValue);
            rewindSound.Play();
        }
        else
        {
            if (RewindManager.Instance.HowManySecondsAvailableForRewind > rewindValue)      //Safety check so it is not grabbing values out of the bounds
                RewindManager.Instance.SetTimeSecondsInRewind(rewindValue);
        }
        isRewinding = true;
    }
}
