using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    float timer = 1.5f;

    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationTriggerCalled()
    {
        base.AnimationTriggerCalled();
    }

    public override void Enter()
    {
        base.Enter();
        timer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            CheckpointHandler.instance.LastCheckPoint.SpawnPlayer(player.gameObject.transform);
            PlayerStats stat = player.Stats as PlayerStats;
            stat.ResetPlayer();
            GameManager.instance.ReduceLives();
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
