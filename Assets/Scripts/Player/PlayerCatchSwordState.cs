using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.sword.transform.position.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        if (player.sword.transform.position.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImapct * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
