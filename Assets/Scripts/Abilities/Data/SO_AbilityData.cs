using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SO_AbilityData : ScriptableObject
{
    public Abilities abilityName;
    public string description;
    public Sprite icon;
    public bool canBeCancelled = false;
}
