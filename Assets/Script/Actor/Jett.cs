using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jett : Actor
{
    private enum AttackState
    {
        Ghost,
        Operator,
    }

    private AttackState _attackState = AttackState.Ghost;

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }
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

    protected override void OnSkill_1()
    {
        if (_isSkill_1Input)
        {
            if (_attackState == AttackState.Ghost)
            {
                _attackState = AttackState.Operator;
            }
            else
            {
                _attackState = AttackState.Ghost;
            }
        }
    }
}
