using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    GameObject target;
    public float speed;
    Rigidbody2D bulletRb;



    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 movDir = (target.transform.position - transform.position).normalized * speed;
        bulletRb.velocity = new Vector2(movDir.x,movDir.y);
        Destroy(this.gameObject,2f);
    }

    
}
