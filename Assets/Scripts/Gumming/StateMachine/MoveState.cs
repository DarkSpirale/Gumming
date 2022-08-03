using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float ceilingDetectedTime;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingCeiling;

    private bool shouldDetectCeiling; //Avoid multiple turnarounds each frame

    public MoveState(Gumming _gumming, string _animBoolName) : base(_gumming, _animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = gumming.CheckIfGrounded();
        isTouchingWall = gumming.CheckIfWall();
        isTouchingCeiling = gumming.CheckIfCeiling();
    }

    public override void Enter()
    {
        base.Enter();

        IsSelectable = true;
        shouldDetectCeiling = true;
        gumming.SetGravity(data.gravityOnGround);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isGrounded)
        {
            stateMachine.ChangeState(gumming.FallState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isTouchingWall)
        {
            gumming.ChangeDirection();
        } else if (isTouchingCeiling && shouldDetectCeiling)
        {
            shouldDetectCeiling = false;
            gumming.ChangeDirection();
            ceilingDetectedTime = Time.time;
        }

        if (!shouldDetectCeiling && Time.time >= ceilingDetectedTime + data.ceilingCheckCooldown)
        {
            shouldDetectCeiling = true;
        }

        gumming.SetVelocityX(data.walkSpeed * gumming.FacingDirection * Time.fixedDeltaTime);
    }
}
