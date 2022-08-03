using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGummingData", menuName = "Data/Gumming Data/Base Data")]
public class SO_GummingData : ScriptableObject
{
    public float walkSpeed = 30f;
    public float gravityOnGround = 0.2f;
    public float gravityInAir = 1.0f;

    public float groundCheckDistance = 0.1f;
    public float wallCheckDistance = 0.1f;
    public float ceilingCheckDistance = 0.1f;
    public float ceilingCheckCooldown = 0.2f;

    public float deathVelocityThreshold = 3f;
    public GameObject deathParticles;

    public LayerMask worldLayer;

    public float outlineThickness = 0.0075f;

}
