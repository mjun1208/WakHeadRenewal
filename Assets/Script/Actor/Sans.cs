using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans : Actor
{
    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("BoneAttack", RpcTarget.All);
    }

    [PunRPC]
    public void BoneAttack()
    {
        var newbone = Global.PoolingManager.LocalSpawn("Sans_Bone", this.transform.position, Quaternion.identity, true);

        newbone.GetComponent<Sans_Bone>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
    }
}
