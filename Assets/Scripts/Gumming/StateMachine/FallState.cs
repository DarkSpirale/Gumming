using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    private bool isGrounded;

    public FallState(Gumming _gumming, string _animBoolName) : base(_gumming, _animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = gumming.CheckIfGrounded(); 
    }

    public override void Enter()
    {
        base.Enter();

        gumming.SetGravity(data.gravityInAir);
        gumming.SetVelocityX(0.0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded && gumming.CurrentVelocity.y >= -data.deathVelocityThreshold)
        {
            stateMachine.ChangeState(gumming.MoveState);
        }
        else if (isGrounded && gumming.CurrentVelocity.y < -data.deathVelocityThreshold)
        {
            stateMachine.ChangeState(gumming.DeadState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
