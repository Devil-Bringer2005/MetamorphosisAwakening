using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointHandler.instance.LastCheckPoint = this;
        }
    }

    public void SpawnPlayer(Transform character)
    {
        character.transform.position = transform.position;
    }
}
