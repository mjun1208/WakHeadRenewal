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

    private Vector3 _throwPosition;
    private Vector3 _throwDir;

    private bool _onSniping = false;

    private List<BattleGround_Throw> _throwList = new List<BattleGround_Throw>();

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
        var newThrow = Global.PoolingManager.LocalSpawn($"BattleGround_Throw_{(ThrowType)throwType}", this.transform.position, Quaternion.identity, true);
        var newThrowScript = newThrow.GetComponent<BattleGround_Throw>();
        newThrowScript.SetInfo(this.photonView, this.gameObject, GetAttackDir(), (ThrowType)throwType);

        newThrowScript.DestoryAction += DespawnThrow;
        _throwList.Add(newThrowScript);
    }

    [PunRPC]
    public void SnipeRPC()
    {
        var newBullet = Global.PoolingManager.LocalSpawn("BattleGround_Aim", this.transform.position, Quaternion.identity, true);

        newBullet.GetComponent<BattleGround_Aim>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }

    public void DespawnThrow(ActorSub throws)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        throws.DestoryAction -= DespawnThrow;

        photonView.RPC("DespawnThrowRPC", RpcTarget.All, GetThrowIndex(throws as BattleGround_Throw));
    }

    private int GetThrowIndex(BattleGround_Throw targetThrow)
    {
        int index = 0;

        foreach (var throws in _throwList)
        {
            if (throws == targetThrow)
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    [PunRPC]
    public void DespawnThrowRPC(int count)
    {
        var targetThrow = _throwList[count];

        _throwList.Remove(targetThrow);

        Global.PoolingManager.LocalDespawn(targetThrow.gameObject);
    }
}
