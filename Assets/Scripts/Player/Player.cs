using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy {  get; private set; }

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float swordReturnImapct;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    [HideInInspector] public int jumpCount = 2;

    [Header("Attack info")]
    public Vector2[] attackMovement;
    public float counterDuration = 0.2f;

    [Header("Dash info")]
    public float dashDuration = 0.2f;
    public float dashSpeed = 25f;
    public float dashDir = 1f;
    public float dashTimer = 0f;
    public float dashCooldown = 3f;
    private float defaultDashSpeed;

    public PlayerStateMachine StateMachine { get; private set; }

    public SkillManager Skill {  get; private set; }

    public AbilityHandler AbilityHandler { get; private set; }

    public GameObject sword { get; private set; }

    #region States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlide { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }

    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }

    public PlayerDeathState DeathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        AirState  = new PlayerAirState(this, StateMachine, "Jump");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");

        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
        PrimaryAttack = new PlayerPrimaryAttackState(this, StateMachine, "Attack");

        AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        BlackholeState = new PlayerBlackholeState(this, StateMachine, "Jump");

        DeathState = new PlayerDeathState(this, StateMachine, "Death");
    }

    private void OnEnable()
    {
        RewindManager.Instance.OnRewindState += HandleRewind;
    }

    private void OnDisable()
    {
        RewindManager.Instance.OnRewindState -= HandleRewind;
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
        Skill = SkillManager.instance;
        AbilityHandler = AbilityHandler.instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        StateMachine.currentState.Update();
        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.C) && AbilityHandler.crystalPermitted)
        {
            Skill.crystal.CanBeUsed();
        }

        if (Input.GetKeyDown(KeyCode.Z) && AbilityHandler.SlowPermitted)
        {
            Skill.slow.CanBeUsed();
        }
    }

    private void HandleRewind(bool status)
    {
        if (status)
        {
            Rb.gravityScale = 0f;
        }
        else
        {
            Rb.gravityScale = 3.5f;
            if (IsGroundDetected())
            {
                Anim.SetBool("Idle", true);
                Anim.SetBool("Move", false);
            }
        }
    }

    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanBeUsed() && AbilityHandler.agilityPermitted)
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
            {
                dashDir = facingDir;
            }
            StateMachine.ChangeState(DashState);
        }
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(sword);
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        float slowValue = 1 - _slowPercentage;
        moveSpeed *= slowValue;
        jumpForce *= slowValue;
        dashSpeed *= slowValue;
        Anim.speed *= slowValue;

        Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationTriggerCalled();

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeathState);
    }

}
