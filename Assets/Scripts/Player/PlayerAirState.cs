using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.WallSlide);

        if (Input.GetKeyDown(KeyCode.Space) && player.jumpCount > 0 && player.AbilityHandler.agilityPermitted)
        {
            player.jumpCount--;
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
        if(xInput == 0)
        {
            return;
        }

        player.SetVelocity(xInput*player.moveSpeed * 0.8f, yInput);
    }
}
