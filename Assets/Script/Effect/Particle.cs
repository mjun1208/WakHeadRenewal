﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Particle : MonoBehaviour
    {
        [SerializeField] protected ParticleSystem _particle;

        // Start is called before the first frame update
        void Start()
        {
            _particle.Play();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (!_particle.isPlaying)
            {
                _particle.Stop();
                Global.PoolingManager.Despawn(this.gameObject);
            }
        }
    }
}
