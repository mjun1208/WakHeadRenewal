using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VR : Actor
{
    protected override void ForceStop(bool isStun)
    {
        base.ForceStop(isStun);

        if (isStun)
        {
            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetList = _skill_1Range.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.Damaged(target.transform.position);
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (_isAttackInput)
        {
            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }
    }

    public override void OnSkill_1()
    {
        base.OnSkill_1();

        photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
    }

    public override void OnSkill_2()
    {
        isSkill_2 = true;
        IsDoingSkill = true;

        photonView.RPC("InvisibilityRPC", RpcTarget.All);
    }

    [PunRPC]
    public void InvisibilityRPC()
    {
        _animator.Rebind();

        float targetAlpha = photonView.IsMine ? 0.5f : 0f;

        _renderer.DOColor(new Color(1, 1, 1, targetAlpha), 0.8f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            isSkill_2 = false;
            IsDoingSkill = false;
        });
    }

    [PunRPC]
    public void DisInvisibilityRPC()
    {
        _renderer.color = new Color(1, 1, 1, 1);
    }
}
