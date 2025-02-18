using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private float defaultGravity;
    private bool skillUsed;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationTriggerCalled()
    {
        base.AnimationTriggerCalled();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.Rb.gravityScale;
        stateTimer = flyTime;
        skillUsed = false;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.Rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 12); 

        if(stateTimer <= 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if(!skillUsed)
            {
                if (player.skill.blackhole.CanBeUsed())
                    skillUsed = true;
            }
        }

        if (player.skill.blackhole.SKillCompleted())
            stateMachine.ChangeState(player.AirState);
    }
}
