using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    public EnemyGroundedState(Enemy _baseEnemy, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_baseEnemy, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < 4)
            stateMachine.ChangeState(enemy.BattleState);

        if (Input.GetKeyDown(KeyCode.K))
            stateMachine.ChangeState(enemy.StunnedState);
    }
}
