using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy_AdvancedAI _baseEnemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunnedDuration;
        rb.velocity = new(enemy.stunnedDistance.x * -enemy.facingDir, enemy.stunnedDistance.y);
        enemy.entityFX.InvokeRepeating("RedWhiteBlink", 0, .1f); ;
        enemy.entityFX.RedWhiteBlink();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.entityFX.CancelColorChange();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
