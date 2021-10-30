using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri_Heart : ActorSub
{
    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _moveSpeed = Constant.AHRI_HEART_MOVE_SPEED;

        StartCoroutine(Go());
    }

    private void Update()
    {
        _attackRange.AttackEntity(targetEntity =>
        {
            OnDamage(targetEntity, 10);
        }, true);
        _attackRange.AttackSummoned(targetSummoned =>
        {
            targetSummoned.Damaged(targetSummoned.transform.position);
            OnDamage(null, 10);
        }, true);
    }
}
