using Photon.Pun;
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

    private ChimpanzeeState _state = ChimpanzeeState.Move;
    private Entity _targetEntity;
    private Tower _targetTower;

    private Vector3 _originalScale = Vector3.zero;

    private const float moveSpeed = 1.5f;

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

    public void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void Start()
    {
        switch (_team)
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

        switch (_state)
        {
            case ChimpanzeeState.Move:
                {
                    Move();

                    if (_attackRange.CollidedObjectList.Count > 0)
                    {
                        _state = ChimpanzeeState.Attack;

                        _animator.SetBool("isAttack", true);
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
        // var targetList = _attackRange.CollidedObjectList;
        // 
        // foreach (var target in targetList)
        // {
        //     //var targetEntity = target.GetComponent<Entity>();
        //     //targetEntity.Damaged(targetEntity.transform.position);
        //     // targetEntity.KnockBack(GetAttackDir(), 0.5f, 0);
        // }
    }
}
