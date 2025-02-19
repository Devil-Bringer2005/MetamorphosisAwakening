using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyAI_1 : MonoBehaviour
{


    public float speed;
    public float circleRadius;   
    private Rigidbody2D enemyRb;
    public GameObject groundCheck;
    public LayerMask groundLayer;

    public bool facingRight;
    public bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
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
}
