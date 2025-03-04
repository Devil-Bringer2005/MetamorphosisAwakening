using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Skill.clone.CreateCloneOnDashStart();
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.Skill.clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDir,0);
        if(!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallSlide);
        }
        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
