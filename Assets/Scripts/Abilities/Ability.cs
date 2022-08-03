using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public SO_AbilityData data;
    public int numberOfUses;    

    public Ability(SO_AbilityData _data, int _amount)
    {
        data = _data;
        numberOfUses = _amount;
    }
}

public enum Abilities
{
    None,
    SpinDrill,
    Dig
}
