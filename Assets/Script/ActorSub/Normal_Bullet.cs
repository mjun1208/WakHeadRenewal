using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Bullet : ActorSub
{
    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _moveSpeed = Constant.NORMAL_BULLET_MOVE_SPEED;

        StartCoroutine(Go());
    }

    private void Update()
    {
        _attackRange.AttackEntity(targetEntity =>
        {
            OnDamage(targetEntity, 30);
        }, true);
        _attackRange.AttackSummoned(targetSummoned =>
        {
            if (_ownerPhotonView.IsMine)
            {
                targetSummoned.Damaged(targetSummoned.transform.position);
            }
            OnDamage(null, 30);
        }, true);
    }

    protected override void OnDamage(Entity entity, int damage)
    {
        StopAllCoroutines();

        if (_ownerPhotonView.IsMine)
        {
            entity?.KnockBack(damage, _dir, 3f, 0f);
        }

        Destroy();
    }
}
