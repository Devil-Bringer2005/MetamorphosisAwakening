using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Animator anim;

    private bool canRotate = true;
    private bool isReturning;

    private float returnungSpeed;

    [Header("Bounce info")]
    private bool isBouncing;
    private int bounceAmount = 4;
    private float bouncingSpeed = 20f;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Spin info")]
    private bool isSpinning;
    [SerializeField] private float maxDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinTimer;
    [SerializeField] private float hitCooldown;
    [SerializeField] private float hitTimer;
    private float spinDirection;
    private bool wasStopped;

    private float freezeDuration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void SetUpSword(Vector2 dir, float _gravityScale, Player _player, float _freezeDuration, float _returningSpeed)
    {
        player = _player;
        rb.velocity = dir;
        rb.gravityScale = _gravityScale;
        freezeDuration = _freezeDuration;
        returnungSpeed = _returningSpeed;

        enemyTarget = new List<Transform>();

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        if(pierceAmount <=0)
            anim.SetBool("Rotation", true);

        Invoke("DestroyMe", 7);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bouncingSpeed = _bounceSpeed;
    }

    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetUpSpin(bool _isSpinning, float _maxDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        spinDuration = _spinDuration;
        maxDistance = _maxDistance;
        hitCooldown = _hitCooldown;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnungSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        SpinLogic();

        BounceLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

        }

        if (wasStopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
            spinTimer -= Time.deltaTime;
            if (spinTimer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }

            hitTimer -= Time.deltaTime;

            if (hitTimer < 0)
            {
                hitTimer = hitCooldown;
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1f);

                foreach (var hit in enemies)
                {
                    if (hit.TryGetComponent(out Enemy_AdvancedAI enemy))
                    SwordSkillDamage(enemy);
                }
            }
        }
    }

    private void SwordSkillDamage(Enemy_AdvancedAI enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine("FreezeTimer", freezeDuration);
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bouncingSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy_AdvancedAI>());
                targetIndex++;
                bounceAmount--;

                if (bounceAmount < 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) { return; }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        if(collision.TryGetComponent(out Enemy_AdvancedAI enemy))
            SwordSkillDamage(enemy);

        SetupForBounce(collision);

        StuckInto(collision);
    }

    private void SetupForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy_AdvancedAI>() != null)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 10);

            foreach (var hit in enemies)
            {
                if (hit.GetComponent<Enemy_AdvancedAI>() != null)
                    enemyTarget.Add(hit.transform);
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy_AdvancedAI>())
        {
            pierceAmount--;
            return;
        }

        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        if (isBouncing && enemyTarget.Count > 0) { return; }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}

