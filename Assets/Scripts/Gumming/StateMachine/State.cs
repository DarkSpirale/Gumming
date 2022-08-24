using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public bool IsSelectable { get; protected set; }

    protected Gumming gumming;
    protected StateMachine stateMachine;
    protected SO_GummingData data;

    protected float startTime;

    protected bool isExitingState; 

    private readonly string animBoolName;

    public State(Gumming _gumming, string _animBoolName)
    {
        gumming = _gumming;
        stateMachine = gumming.StateMachine;
        data = gumming.Data;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        isExitingState = false;
        gumming.Animator.SetBool(animBoolName, true);
        startTime = Time.time;
        DoChecks();
    }

    public virtual void Exit()
    {
        isExitingState = true;
        gumming.Animator.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        if (isExitingState) return;
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

}