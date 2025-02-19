using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }

        if(!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (xInput != 0)
        {
            if (xInput != player.facingDir)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(xInput ==  player.facingDir)
            {
                player.SetVelocity(0, 0.1f * yInput);
            }
        }
        else
        {
            player.SetVelocity(0, yInput * 0.9f);
        }
        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
