using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoned : ActorSub
{
    private int _maxHP;
    private int _currentHP;

    public bool IsDead { get; protected set; } = false;

    public int MaxHP
    {
        get
        {
            return _maxHP;
        }
        protected set
        {
            _maxHP = value;
        }
    }

    public int HP
    {
        get
        {
            return _currentHP;
        }
        protected set
        {
            _currentHP = value;
        }
    }

    public void Damaged(Vector3 pos)
    {
        if (photonView == null)
        {
            return;
        }

        photonView.RPC("OnDamageRPC", RpcTarget.All, pos);
    }

    [PunRPC]
    public void OnDamageRPC(Vector3 pos)
    {
        OnDamage(pos);
    }

    public void OnDamage(Vector3 pos)
    {
        var randomPos = (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f;

        Global.PoolingManager.LocalSpawn("HitEffect", this.transform.position + randomPos, this.transform.rotation, true);

        if (photonView.IsMine)
        {
            _currentHP--;

            if (_currentHP <= 0)
            {
                IsDead = true;
            }
        }
    }
}
