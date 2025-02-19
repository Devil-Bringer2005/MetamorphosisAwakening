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
    
    void FixedUpdate()
    {
        if(Input.GetKey(keyToRewind) && AbilityHandler.instance.rewindPermitted)                   
        {
            StartRewind();
        }
        else
        {
            if(isRewinding)
            {
                StopRewind();
            }
        }
    }

    private void StopRewind()
    {
        RewindManager.Instance.StopRewindTimeBySeconds();
        rewindSound.Stop();
        rewindValue = 0;
        isRewinding = false;
    }

    private void StartRewind()
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
