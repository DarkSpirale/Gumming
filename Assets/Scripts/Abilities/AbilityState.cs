using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityState<T> : State where T : SO_AbilityData
{
    protected T abilityData;

    public AbilityState(Gumming _gumming, string _animBoolName) : base(_gumming, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        gumming.SetCurrentAbility(abilityData.abilityName);
        IsSelectable = abilityData.canBeCancelled;
    }

    public override void Exit()
    {
        base.Exit();

        gumming.SetCurrentAbility(Abilities.None);
    }
}
