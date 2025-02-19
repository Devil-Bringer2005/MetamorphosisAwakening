using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonDeathState(Enemy_AdvancedAI _baseEnemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void AnimationTriggerCalled()
    {
        base.AnimationTriggerCalled();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.Anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.Anim.speed = 0;
        enemy.Collider.enabled = false;

        stateTimer = 0.15f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(2, 20);
        }
    }
}
