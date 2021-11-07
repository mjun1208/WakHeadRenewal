using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGround : Actor
{
    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _attackRange.Attack(targetEntity =>
        {
            targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0);
        });
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }
}
