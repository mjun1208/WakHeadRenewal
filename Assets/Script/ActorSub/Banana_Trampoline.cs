using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana_Trampoline : Summoned
    {
        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            transform.position = pos;
            
            MaxHP = 5;
            HP = MaxHP;
        }
    }
}
