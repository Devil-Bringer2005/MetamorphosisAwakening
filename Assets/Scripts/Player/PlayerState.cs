using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    private string animBoolName;

    public float xInput;
    public float yInput;

    protected float stateTimer;
    public bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        stateMachine = _stateMachine;
        player = _player;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(animBoolName, true);
        rb = player.Rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = rb.velocity.y;
        player.Anim.SetFloat("yVelocity", yInput);

        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit() 
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationTriggerCalled()
    {
        triggerCalled = true;
    }
}
