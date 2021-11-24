using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Dummy : Entity
    {
        protected override void Awake()
        {
            base.Awake();

            MaxHP = 99999;
            ResetHp();
        }

        private void Update()
        {
            HP = 99999;
        }
    }
}