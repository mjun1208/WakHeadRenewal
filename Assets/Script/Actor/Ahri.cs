﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri : Actor
{
    [SerializeField] private GameObject OrbPrefab;
    [SerializeField] private GameObject HeartPrefab;

    private GameObject MyOrb;

    private float _rushSpeed = 15f;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_isDoingSkill && _isSkill_2)
        {
            SpiritRush();
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("ShootHeart", RpcTarget.All);
    }

    protected override void Attack()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return;
        }

        _animator.SetBool("IsAttack", false);

        if (_isAttackInput)
        {
            if (MyOrb != null)
            {
                if (!MyOrb.activeSelf)
                {
                    MyOrb = null;
                }
            }

            if (MyOrb == null)
            {
                photonView.RPC("ShootOrb", RpcTarget.All);
            }
        }
    }

    private void SpiritRush()
    {
        _rigid.MovePosition(transform.position + GetAttackDir() * _rushSpeed * Time.deltaTime);
    }

    [PunRPC]
    public void ShootOrb()
    {
        MyOrb = Global.PoolingManager.LocalSpawn("Ahri_Orb", this.transform.position, Quaternion.identity, true);

        base.Attack();
        MyOrb.SetActive(true);
        MyOrb.GetComponent<Ahri_Orb>().SetInfo(this.gameObject, GetAttackDir());
    }

    [PunRPC]
    public void ShootHeart()
    {
        var newHeart = Global.PoolingManager.LocalSpawn("Ahri_Heart", this.transform.position, Quaternion.identity, true);

        newHeart.GetComponent<Ahri_Heart>().SetInfo(this.gameObject, GetAttackDir());
    }
}
