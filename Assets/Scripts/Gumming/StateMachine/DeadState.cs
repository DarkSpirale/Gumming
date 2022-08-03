using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public DeadState(Gumming _gumming, string _animBoolName) : base(_gumming, _animBoolName)
    {
    }

    public override void Enter()
    {
        GameObject.Instantiate(data.deathParticles, gumming.transform.position, data.deathParticles.transform.rotation);
        GameObject.Destroy(gumming.gameObject);
    }
}
