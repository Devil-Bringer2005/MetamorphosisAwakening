using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsEnnemy;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D circleCol => GetComponent<CircleCollider2D>();

    private Transform closestEnemy;

    private float crystalTimer;

    private bool canExplode;
    private bool canMove;
    private bool canGrow;

    private float moveSpeed;
    private float growSpeed = 5f;

    public void SetUpCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        crystalTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        if (crystalTimer < 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3),  growSpeed * Time.deltaTime);
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                canMove = false;
                FinishCrystal();
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnnemy);

        if(col.Length > 0)
            closestEnemy =  col[Random.Range(0, col.Length)].transform;
    }

    private void AnimationExplosionTrigger()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, circleCol.radius);

        foreach(Collider2D hit in col)
        {
            if(hit.TryGetComponent(out Enemy enemy))
            {
                enemy.DamageEffect();
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
