using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class LocalParticle : Particle
    {
        public override void Update()
        {
            if (!_particle.isPlaying)
            {
                _particle.Stop();
                Global.PoolingManager.LocalDespawn(this.gameObject);
            }
        }
    }
}