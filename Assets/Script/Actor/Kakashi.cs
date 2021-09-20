using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi : Actor
{
    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetList = _skill_1Range.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.KnockBack(GetAttackDir(), 1f, 0);
        }
    }
}
