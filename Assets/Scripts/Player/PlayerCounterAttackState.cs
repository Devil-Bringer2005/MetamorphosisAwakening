using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterDuration;
        player.Anim.SetBool("SuccessfulCounterAttack", false);
        canCreateClone = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] col = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach(var hit in col)
        {
            if(hit.TryGetComponent(out Enemy enemy))
            {
                if(enemy.CanBeStunned())
                {
                    stateTimer = 10f;
                    player.Anim.SetBool("SuccessfulCounterAttack", true);

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.clone.CreateCloneOnCounterAttack(enemy.transform);
                    }
                }
            }
        }

        if(stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
