using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Actor
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _attackRange.Attack(targetEntity => 
        {
            targetEntity.KnockBack(GetAttackDir(), 0.5f, 0);
        });
    } 

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _skill_1Range.Attack(targetEntity =>
        {
            targetEntity.Grab(this.transform.position, 10f);
        });
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("ShootBullet", RpcTarget.All);
    }

    [PunRPC]
    public void ShootBullet()
    {
        var newBullet = Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

        newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }
}
