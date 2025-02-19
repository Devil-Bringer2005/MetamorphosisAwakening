using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy_AdvancedAI _baseEnemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
    }
    public override void Update()
    {
        base.Update();

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else
            moveDir = -1;
        
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);

        if (enemy.PlayerDetected())
        {
            if(enemy.PlayerDetected().distance <= enemy.attackDistance)
            {
                if(CanAttack())
                stateMachine.ChangeState(enemy.AttackState);
            }
        }
        else
        {
            if(stateTimer <= 0 || Vector2.Distance(player.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.IdleState);
            }
        }
            
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack()
    {
        if(Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

}
