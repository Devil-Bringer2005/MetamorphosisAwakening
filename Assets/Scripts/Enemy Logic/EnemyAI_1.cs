using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyAI_1 : MonoBehaviour, ISlowable
{


    public float speed;
    public float circleRadius;   
    private Rigidbody2D enemyRb;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public Animator animator;

    public bool facingRight;
    public bool isGrounded;
    private float defaultSpeed;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        defaultSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.velocity = Vector2.right * speed * Time.deltaTime;
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);


        if (!isGrounded && facingRight )
        {
            Flip();
        }
        else if (!isGrounded && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        speed = -speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position,circleRadius);
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
