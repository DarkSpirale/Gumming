using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

[CreateAssetMenu(fileName = "DigData", menuName = "Data/Ability Data/Dig Data")]
public class SO_DigData : SO_AbilityData
{
    public float diggingSpeed = 5f;
    public float diggingTime = 10f;
    public ShapeName typeOfShape = ShapeName.rectangle;
    public Sprite diggingShape;
    public int diggingSize;
    public int diggingSize2;
    public float intervalBetweenDigSteps = 0.2f;
    public float diggingPosOffset = 0.5f;
    public float posShiftBetweenDigSteps = 0.03125f;

}
