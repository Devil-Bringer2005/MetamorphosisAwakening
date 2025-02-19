using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    public static CheckpointHandler instance;

    public Checkpoint LastCheckPoint;

    private void Awake()
    {
        instance = this;
    }


}
