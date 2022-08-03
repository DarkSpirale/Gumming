using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

[CreateAssetMenu(fileName = "newSpinDrillData", menuName = "Data/Ability Data/Sprin Drill Data")]
public class SO_SpinDrillData : SO_AbilityData
{
    public float drillForce = 10f;
    public float drillDuration = 1f;
    public float linearDrag = 1f;
    public ShapeName typeOfShape = ShapeName.texture;
    public Sprite drillingShape;
    public int drillingSize;
}
