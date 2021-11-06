using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Entity
{
    protected override void Awake()
    {
        base.Awake();

        MaxHP = 99999;
        HP = MaxHP;
    }

    private void Update()
    {
        HP = 99999;
    }
}
