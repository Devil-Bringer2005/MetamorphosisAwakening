using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootPlayer : MonoBehaviour
{
    
    public float speed;
    public float lineOfSight;
    public float shootingRange;
    public float fireRate = 1f;
    private float nextFireTime;
    private Animator enemyAnim;
    public GameObject bullet;
    public GameObject bulletSpawn;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
        
    }
    void Update()
    {
        MoveTowardsPlayer();
    }




    void MoveTowardsPlayer()
    {
        float distanceFromplayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromplayer < lineOfSight && distanceFromplayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromplayer <= shootingRange && nextFireTime < Time.time)
        {
            enemyAnim.SetTrigger("Aggro");
            Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }

    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootingRange);

    }
}
