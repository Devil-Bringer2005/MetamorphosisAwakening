using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && player.jumpCount > 0 && player.AbilityHandler.agilityPermitted)
        {
            player.jumpCount--;
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        if (yInput <= 0)
        {
            stateMachine.ChangeState(player.AirState);
        }
        player.SetVelocity(xInput * player.moveSpeed * 0.8f, yInput);
    }

}
