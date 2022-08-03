using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class SpinDrillState : State
{
    protected SO_SpinDrillData abilityData;

    private Shape drillingShape;

    private float startPosX;
    private float endPosX;

    private bool isGrounded;

    public SpinDrillState(Gumming _gumming, SO_SpinDrillData _abilityData, string _animBoolName) : base(_gumming, _animBoolName)
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

        gumming.SetColliderEnabled(false);
        gumming.SetLinearDrag(abilityData.linearDrag);
        gumming.SetGravity(0f);
        gumming.SetVelocity(Vector2.zero);
        gumming.AddForce(abilityData.drillForce, gumming.FacingDirection * Vector2.right);

        GenerateDrillingShape();

        startPosX = gumming.transform.position.x;
    }

    public override void Exit()
    {
        base.Exit();

        gumming.SetLinearDrag(0f);
        gumming.SetColliderEnabled(true);

        endPosX = gumming.transform.position.x;
        float distanceTravelled = Mathf.Abs(endPosX - startPosX);
        //Debug.Log("Distance travelled while drilling : " + distanceTravelled);

        gumming.SetCurrentAbility(Abilities.None);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + abilityData.drillDuration)
        {
            if (isGrounded) stateMachine.ChangeState(gumming.MoveState);
            else stateMachine.ChangeState(gumming.FallState);    
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gumming.SetVelocityY(0f);
        WorldManager.instance.DestroyShape(drillingShape, gumming.transform.position);
    }

    private void GenerateDrillingShape()
    {
        int drillingSize = abilityData.drillingSize;

        switch (abilityData.typeOfShape)
        {
            case ShapeName.circle:
                drillingShape = Shape.GenerateShapeCircle(drillingSize);
                break;
            case ShapeName.rectangle:
                drillingShape = Shape.GenerateShapeRect(drillingSize, drillingSize);
                break;
            case ShapeName.texture:
                drillingShape = Shape.GenerateShapeFromSprite(abilityData.drillingShape);
                break;
        }
    }
}
