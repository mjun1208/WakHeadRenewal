using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks , IPunObservable
{
    [SerializeField] private Team _team;

    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            if (_hp != value)
            {
                _hpDownAction.Invoke();
                _hp = value;
            }
        }
    }
    
    public int _maxHp;
    private int _hp;

    private Action _hpDownAction;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HP);
        }
        else
        {
            HP = (int)stream.ReceiveNext();
        }
    }
    
    private void Awake()
    {
        _maxHp = 20;
        _hp = 20;
        
        switch (_team)
        {
            case Team.BLUE:
                {
                    Global.instance.SetBlueTower(this);
                    break;
                }
            case Team.RED:
                {
                    Global.instance.SetRedTower(this);
                    break;
                }
        }

        _hpDownAction += SpawnHitEffect;
    }

    public void OnDamage()
    {
        HP -= 1;
    }

    public void SpawnHitEffect()
    {
        switch (_team)
        {
            case Team.BLUE:
            {
                var hitEffect = Global.PoolingManager.LocalSpawn("BlueTowerHit", this.transform.position, this.transform.rotation, true);
                break;
            }
            case Team.RED:
            {
                var hitEffect = Global.PoolingManager.LocalSpawn("RedTowerHit", this.transform.position, this.transform.rotation, true);
                break;
            }
        }
    }
}
