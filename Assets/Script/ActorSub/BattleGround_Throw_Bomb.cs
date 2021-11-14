using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGround_Throw_Bomb : ActorSub
{
    private BattleGround.ThrowType _throwType;

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, BattleGround.ThrowType throwType)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _throwType = throwType;

        if (_throwType == BattleGround.ThrowType.MOLOTOV)
        {
            pos += new Vector3(0, 1f, 0);
        }

        this.transform.position = pos;
    }

    private void OnDamage()
    {
        if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
        {
            return;
        }

        _attackRange.Attack(targetEntity =>
        {
            targetEntity.Damaged(this.transform.position, 5);
        });
    }
}
