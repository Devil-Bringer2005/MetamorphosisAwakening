using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.AirState);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(player.BlackholeState);
        }

        //if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        //{
        //    stateMachine.ChangeState(player.AimSwordState);
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.CounterAttackState);
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() && player.jumpPermitted)
        {
            stateMachine.ChangeState(player.JumpState);
        }

        //if(Input.GetKey(KeyCode.Mouse0))
        //{
        //    stateMachine.ChangeState(player.PrimaryAttack);
        //}
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

}