﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimpanzee : Entity, IPunObservable
{
    private enum ChimpanzeeState
    {
        Move,
        Attack
    }

    [SerializeField] protected Animator _animator;
    [SerializeField] protected AttackRange _attackRange;

    private const float moveSpeed = 1.5f;

    private ChimpanzeeState _state = ChimpanzeeState.Move;
    private Entity _targetEntity;
    private Tower _targetTower;

    private Vector3 _originalScale = Vector3.zero;

    private  float _attackDelay = 0;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.localScale.x);
        }
        else
        {
            var scale_x = (float)stream.ReceiveNext();
            this.transform.localScale = new Vector3(scale_x, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _originalScale = this.transform.localScale;

        CrownControlAction += () => {
            _state = ChimpanzeeState.Move;

            _animator.SetBool("isAttack", false);

            _rigid.isKinematic = false;

            _attackDelay = 0.5f;
        };

        MaxHP = 50;
        HP = MaxHP;

        DeadAction += Dead;
    }

    public void Init(Team team)
    {
        SetTeam(team);

        switch (MyTeam)
        {
            case Team.BLUE:
                {
                    _targetTower = Global.instance.RedTower;

                    break;
                }
            case Team.RED:
                {
                    _targetTower = Global.instance.BlueTower;

                    break;
                }
        }

        _attackRange.SetTeam(MyTeam);
    }

    public void SetTarget(Entity entity)
    {
        _targetEntity = entity;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (_attackDelay > 0)
        {
            _attackDelay -= Time.deltaTime;
        }
        else
        {
            _attackDelay = 0f;
        }

        switch (_state)
        {
            case ChimpanzeeState.Move:
                {
                    if (_attackDelay <= 0)
                    {
                        Move();

                        if (_attackRange.CollidedObjectList.Count > 0)
                        {
                            _state = ChimpanzeeState.Attack;

                            _animator.SetBool("isAttack", true);

                            _rigid.isKinematic = true;
                        }
                    }
                    break;
                }
            case ChimpanzeeState.Attack:
                {
                    Attack();

                    if (_attackRange.CollidedObjectList.Count == 0)
                    {
                        _state = ChimpanzeeState.Move;

                        _animator.SetBool("isAttack", false);

                        _rigid.isKinematic = false;
                    }
                    break;
                }
        }
    }

    private void Move()
    {
        if (_targetEntity == null)
        {
            Vector3 dir = _targetTower.transform.position - this.transform.position;
            dir = dir.normalized;

            if (dir.x != 0)
            {
                float rotation = 0;
                if (dir.x > 0)
                {
                    rotation = 1;
                }
                else
                {
                    rotation = -1;
                }
                float rotationScale = _originalScale.x * rotation;
                this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            }

            _rigid.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        _attackRange.Attack(targetEntity =>
        {
            targetEntity.Damaged(targetEntity.transform.position, 2);
            //targetEntity.KnockBack(GetAttackDir(), 0.5f, 0);
        });
    }

    private void Dead()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        var deathEffect = Global.PoolingManager.LocalSpawn("DeathEffect", this.transform.position, this.transform.rotation, true);
    }
}
