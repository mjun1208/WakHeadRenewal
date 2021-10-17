using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft : Actor
{
    [SerializeField] private GameObject _lavaPivot;

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
        var newLavaBasket = Global.PoolingManager.LocalSpawn("Minecraft_LavaBasket", _lavaPivot.transform.position, Quaternion.identity, true);
        var newLava = Global.PoolingManager.LocalSpawn("Minecraft_Lava", _lavaPivot.transform.position, Quaternion.identity, true);

        newLava.GetComponent<Minecraft_Lava>().SetInfo(this.photonView, null, _lavaPivot.transform.position, GetAttackDir());
    }
}

