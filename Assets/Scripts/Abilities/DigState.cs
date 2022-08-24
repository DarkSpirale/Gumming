using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class DigState : AbilityState<SO_DigData>
{
    private Shape diggingShape;

    private Vector2 workspace;

    private float digStepStartTime;

    private bool isGrounded;

    public DigState(Gumming _gumming, SO_DigData _abilityData, string _animBoolName) : base(_gumming, _animBoolName)
    {
        abilityData = _abilityData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = gumming.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        gumming.SetVelocityX(0f);
        gumming.SetRigidbodyType(RigidbodyType2D.Static);

        GenerateDiggingShape();
        Dig();
    }

    public override void Exit()
    {
        base.Exit();

        gumming.SetRigidbodyType(RigidbodyType2D.Dynamic);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        WaitForNextDigStep();

        if (!isGrounded)
        {
            gumming.StateMachine.ChangeState(gumming.FallState);
        }
        else if (Time.time >= startTime + abilityData.diggingTime)
        {
            gumming.StateMachine.ChangeState(gumming.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void WaitForNextDigStep()
    {
        if (Time.time >= digStepStartTime + abilityData.intervalBetweenDigSteps)
        {
            Dig();
        }
    }

    private void Dig()
    {
        digStepStartTime = Time.time;

        workspace.Set(gumming.transform.position.x, gumming.transform.position.y - abilityData.diggingPosOffset);
        WorldManager.instance.DestroyShape(diggingShape, workspace);

        workspace.Set(gumming.transform.position.x, gumming.transform.position.y - abilityData.posShiftBetweenDigSteps);
        gumming.SetPosition(workspace);
    }

    private void GenerateDiggingShape()
    {
        int diggingSize = abilityData.diggingSize;

        switch (abilityData.typeOfShape)
        {
            case ShapeName.circle:
                diggingShape = Shape.GenerateShapeCircle(diggingSize);
                break;
            case ShapeName.rectangle:
                diggingShape = Shape.GenerateShapeRect(diggingSize, abilityData.diggingSize2);
                break;
            case ShapeName.texture:
                diggingShape = Shape.GenerateShapeFromSprite(abilityData.diggingShape);
                break;
        }
    }
}
