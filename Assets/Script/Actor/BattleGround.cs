using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattleGround : Actor
{
    public enum ThrowType
    {
        GRENADE,
        MOLOTOV,
        FLASH_BANG
    }

    private ThrowType _throwType;
    private bool _onSniping = false;

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

        photonView.RPC("ThrowRPC", RpcTarget.All, (int)_throwType);
    }

    public override void OnSkill_1()
    {
        var throwValues = Enum.GetValues(typeof(ThrowType));
        _throwType = (ThrowType)throwValues.GetValue(new Random().Next(0, throwValues.Length));

        _animator.SetInteger("Throws", (int)_throwType);

        IsSkill_1 = true;

        if (OnSkillCoroutine == null)
        {
            IsDoingSkill = true;
            OnSkillCoroutine = OnSkill($"Skill_1_{(int)_throwType}");
            StartCoroutine(OnSkillCoroutine);

            _animator.SetBool($"IsSkill_1_{(int)_throwType}", true);
        }
    }

    public override void OnSkill_2()
    {
        _onSniping = true;

        IsDoingSkill = true;
        IsSkill_2 = true;

        _animator.SetBool("IsSkill_2", true);

        photonView.RPC("SnipeRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ThrowRPC(int throwType)
    {
        var newBullet = Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

        newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }

    [PunRPC]
    public void SnipeRPC()
    {
        var newBullet = Global.PoolingManager.LocalSpawn("BattleGround_Aim", this.transform.position, Quaternion.identity, true);

        newBullet.GetComponent<BattleGround_Aim>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }
}
