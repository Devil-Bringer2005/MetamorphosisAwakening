using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AdvancedAI : Entity, ISlowable
{
    [SerializeField] protected LayerMask playerLayer;
    [Header("Move Info")]
    public float idleTime;
    public float moveSpeed;
    public float battleTime;
    public float defaultMoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunnned info")]
    public Vector2 stunnedDistance;
    public float stunnedDuration;
    public bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    public EnemyStateMachine StateMachine { get; private set; }
    public CapsuleCollider2D Collider {  get; private set; }

    public string lastAnimBoolName;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        Collider = GetComponent<CapsuleCollider2D>();
        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
    }

    #region Counter Window
    public void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;

    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        float slowVallue = 1 - _slowPercentage;
        moveSpeed *= slowVallue;
        Anim.speed *= slowVallue;

        Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public void FreezeEnemy(bool _freeze)
    {
        if(_freeze)
        {
            moveSpeed = 0;
            Anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimer(float _seconds)
    {
        FreezeEnemy(true);
        yield return new WaitForSeconds(_seconds);
        FreezeEnemy(false);
    }

    public RaycastHit2D PlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, playerLayer);
    public void AnimationTrigger() => StateMachine.currentState.AnimationTriggerCalled();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public void Slow(bool status, float multiplier = 1)
    {
        if (status)
        {
            moveSpeed = moveSpeed * multiplier;
            Anim.speed = multiplier;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1;
        }
    }
}
