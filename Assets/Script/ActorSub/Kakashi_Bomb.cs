using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi_Bomb : ActorSub
{
    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;
    }

    private void OnDamage()
    {
        if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
        {
            return;
        }

        foreach (var targetObject in _attackRange.CollidedObjectList)
        {
            var targetEntity = targetObject.GetComponent<Entity>();

            if (!targetEntity.photonView.IsMine)
            {
                targetEntity.Damaged(this.transform.position);
            }
        }
    }
}
