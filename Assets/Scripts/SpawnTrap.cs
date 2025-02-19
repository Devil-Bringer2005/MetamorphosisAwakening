using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrap : MonoBehaviour
{
    public GameObject spawnEnemy; // Enemy prefab to spawn
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the object has the Player tag
        {
            if (spawnEnemy != null)
            {
                animator.SetTrigger("Pop");
                Instantiate(spawnEnemy, transform.position, Quaternion.identity); // Spawn at trap position
                Destroy(gameObject,2f);
            }
            else
            {
                Debug.LogError("SpawnEnemy prefab is not assigned!");
            }
        }
    }
}
