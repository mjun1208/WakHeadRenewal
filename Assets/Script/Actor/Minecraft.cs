using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft : Actor
{
    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("LavaElemant", RpcTarget.All);
    }

    [PunRPC]
    public void LavaElemant()
    {
        var newLava = Global.PoolingManager.LocalSpawn("Minecraft_Lava", this.transform.position, Quaternion.identity, true);

        newLava.GetComponent<Minecraft_Lava>().SetInfo(this.photonView, this.gameObject, this.transform.position, GetAttackDir());
    }
}
