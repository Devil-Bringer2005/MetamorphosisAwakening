using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    public float displayDuration = 5f;
    private void Update()
    {
        displayDuration = Time.deltaTime;

        if (displayDuration < 0 )
            gameObject.SetActive( false );
    }
}
