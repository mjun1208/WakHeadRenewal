﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSub : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigid;
    [SerializeField] protected AttackRange _attackRange;

    protected GameObject _owner;
    protected Vector3 _dir;
    protected PhotonView _ownerPhotonView;

    protected float _moveSpeed = Constant.ACTOR_SUB_DEFAULT_MOVE_SPEED;
    protected float _lifeTime = Constant.ACTOR_SUB_DEFAULT_LIFETIME;

    public Action DestoryAction = null;

    public virtual void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        _ownerPhotonView = ownerPhotonView;

        _attackRange.SetOwner(owner);
        this.transform.position = owner.transform.position;

        _owner = owner;

        _dir = dir;

        DestoryAction += OnDestory;
    }

    protected virtual void OnDamage(Entity entity)
    {
        if (entity.photonView.IsMine)
        {
            return;
        }

        StopAllCoroutines();

        if (_ownerPhotonView.IsMine)
        {
            entity.Damaged(this.transform.position);
        }

        DestoryAction?.Invoke();

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }

    protected virtual IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < _lifeTime)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

            yield return null;
        }

        DestoryAction?.Invoke();

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }

    protected virtual void OnDestory()
    {
        DestoryAction -= OnDestory;
    }
}
