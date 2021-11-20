using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vengenpro : Actor
{
    float _attackPressTime = 0f;
    float _attackPressFullCharingTime = 0f;
    float _attackPressDelay = 0f;

    protected override void Update()
    {
        base.Update();

        if (_isAttack && _attackPressDelay <= 0f)
        {
            _attackPressDelay = 0f;

            if (_attackPressTime < 5f)
            {
                _attackPressTime += Time.deltaTime;
                _animator.SetFloat("AttackSpeed", 1 + _attackPressTime * 0.5f);
            }
            else
            {
                _attackPressFullCharingTime += Time.deltaTime;
            }

            if (_attackPressFullCharingTime >= 1.5f)
            {
                _attackPressFullCharingTime = 0f;
                _attackPressDelay = 5f;
            }
        }
        else
        {
            _attackPressDelay -= Time.deltaTime;

            _attackPressTime = 0;
            _animator.SetFloat("AttackSpeed", 1);
        }
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            photonView.RPC("ShootNote", RpcTarget.All, randomDir);
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("ShootZzang", RpcTarget.All);
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _skill_2Range.AttackEntity(targetEntity =>
        {
            var dir = targetEntity.transform.position - this.transform.position;

            targetEntity.KnockBack(10, dir.normalized, 3f, 1.5f);
        });
    }

    [PunRPC]
    public void ShootNote(Vector2 randomDir)
    {
        var newNote = Global.PoolingManager.LocalSpawn("Vengenpro_Note", this.transform.position, Quaternion.identity, true);

        newNote.GetComponent<Vengenpro_Note>().SetInfo(this.photonView, this.gameObject, randomDir);
    }
    
    [PunRPC]
    public void ShootZzang()
    {
        var newZzang = Global.PoolingManager.LocalSpawn("Vengenpro_Zzang", this.transform.position, Quaternion.identity, true);
        
        newZzang.GetComponent<Vengenpro_Zzang>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }
    
    [PunRPC]
    public void SonicBoom()
    {
        var newSonic = Global.PoolingManager.LocalSpawn("Vengenpro_SonicBoom", this.transform.position, Quaternion.identity, true);
    }
}
