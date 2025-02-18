using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Check info")]
    public Transform attackCheck;
    public float attackRadius;
    public Transform groundCheck;
    public float groundDistance;
    public Transform wallCheck;
    public float wallDistance;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDIr;
    [SerializeField] protected bool isKnocked;
    [SerializeField] protected float knockbackDuration;

    public System.Action onFlipped;

    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public EntityFX entityFX { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CharacterStats Stats { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;

    public LayerMask groundLayer;
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
        entityFX = GetComponent<EntityFX>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Stats = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    { }

    protected virtual void ReturnDefaultSpeed()
    {
        Anim.speed = 1;
    }

    public void DamageEffect()
    {
        StartCoroutine(entityFX.FlashFX());
        StartCoroutine(HitKnockback());
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        Rb.velocity = new(knockbackDIr.x * -facingDir, knockbackDIr.y);
        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }
    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
            return;

        Rb.velocity = new(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void ZeroVelocity()
    {
        if(isKnocked)
            return;
        SetVelocity(0, 0);
    }
    #endregion

    #region Collision Detection
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallDistance, groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new(groundCheck.position.x, groundCheck.position.y - groundDistance));
        Gizmos.DrawLine(wallCheck.position, new(wallCheck.position.x + wallDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion

    #region Flip
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        if (_x < 0 && facingRight)
            Flip();
    }
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(new(0, 180, 0));

        onFlipped?.Invoke();
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if(_transparent)
            spriteRenderer.color = Color.clear;
        else 
            spriteRenderer.color = Color.white;
    }

    public virtual void Die()
    {

    }
}
