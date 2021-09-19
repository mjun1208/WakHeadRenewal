using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalParticle : Particle
{
    public override void Update()
    {
        if (!_particle.isPlaying)
        {
            Global.PoolingManager.LocalDespawn(this.gameObject);
        }
    }
}
