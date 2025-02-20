using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour, ISlowable
{

    public float speed;
    public float wanderSpeed;
    public float circleRadius;

    public float lineOfSight;
    private Transform player;
    private Rigidbody2D enemyRb;
    private Animator animator;
    public LayerMask Wall;
    public GameObject AirCheck;

    public bool isfacingRight;
    public bool isFlyingFreely;

    public bool aggro;
    private float defaultSpeed;


    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        
        animator = GetComponent<Animator>();
        defaultSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aggro)
        {
            Wander();
        }
        MoveTowardsPlayer();
        

        
    }

    void MoveTowardsPlayer()
    {
        float distanceFromplayer = Vector2.Distance(player.position, transform.position); 

        if (distanceFromplayer < lineOfSight)
        {   
            aggro = true;
            animator.SetBool("Aggro", true);
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
       
        else
        {
            aggro= false;
            animator.SetBool("Aggro", false);
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lineOfSight);
        
    }
    

    void Wander()
    {
        Debug.Log(enemyRb == null);
        enemyRb.velocity = Vector2.right * wanderSpeed *Time.deltaTime;
        isFlyingFreely = Physics2D.OverlapCircle(AirCheck.transform.position, circleRadius, Wall);
        
        if (isFlyingFreely && isfacingRight)
        {
            Flip();
        }

        else if (isFlyingFreely && !isfacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isfacingRight = !isfacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        wanderSpeed = -wanderSpeed;
    }

    public void Slow(bool status, float multiplier = 1)
    {
        if (status)
        {
            speed = speed * multiplier;
            animator.speed = multiplier;
        }
        else
        {
            speed = defaultSpeed;
            animator.speed = 1;
        }
    }
}
