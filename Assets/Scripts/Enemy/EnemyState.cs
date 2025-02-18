using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy baseEnemy;
    protected Rigidbody2D rb;

    protected Transform player;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _baseEnemy, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        baseEnemy = _baseEnemy;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player = PlayerManager.instance.player.transform;
        triggerCalled = false;
        rb = baseEnemy.Rb;
        baseEnemy.Anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        baseEnemy.AssignLastAnimBoolName(animBoolName);
        baseEnemy.Anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationTriggerCalled()
    {
        triggerCalled = true;
    }

}
