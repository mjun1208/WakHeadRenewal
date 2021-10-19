using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Naruto_Dummy : ActorSub
{
    [SerializeField] private Animator _animator;
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    private void Active_Attack()
    {
        if (!_ownerPhotonView.IsMine)
        {
            return;
        }

        var targetList = _attackRange.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.KnockBack(_dir, 1.5f, 0);
        }
    }

    public void SetDir(Vector3 dir)
    {
        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
    }

    public void Attack(bool isAttack)
    {
        _animator.SetBool("IsAttack", isAttack);
    }
    
    public void SetAnimationParameter(string name, bool isTrue)
    {
        _animator.SetBool(name, isTrue);
    }

    public void Rasengan()
    {
    }
}
