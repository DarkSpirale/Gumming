using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class DigState : State
{
    protected SO_DigData abilityData;

    private Shape diggingShape;

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

        gumming.SetCurrentAbility(abilityData.abilityName);
        IsSelectable = abilityData.canBeCancelled;

        gumming.SetVelocityX(0f);
        gumming.SetRigidbodyType(RigidbodyType2D.Static);

        GenerateDiggingShape();
        Dig();
    }

    public override void Exit()
    {
        base.Exit();

        gumming.SetRigidbodyType(RigidbodyType2D.Dynamic);

        gumming.SetCurrentAbility(Abilities.None);
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

        Vector2 diggingPos = new Vector2(gumming.transform.position.x, gumming.transform.position.y - abilityData.diggingPosOffset);
        WorldManager.instance.DestroyShape(diggingShape, diggingPos);

        gumming.SetPosition(diggingPos);
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
                diggingShape = Shape.GenerateShapeRect(diggingSize, diggingSize);
                break;
            case ShapeName.texture:
                diggingShape = Shape.GenerateShapeFromSprite(abilityData.diggingShape);
                break;
        }
    }
}
