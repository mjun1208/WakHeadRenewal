using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class LocalEffect : Effect
    {
        public override void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            {
                Global.PoolingManager.LocalDespawn(this.gameObject);
            }
        }
    }
}