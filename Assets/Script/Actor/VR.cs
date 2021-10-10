using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR : Actor
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
            targetEntity.Damaged(target.transform.position);
        }
    }
}
