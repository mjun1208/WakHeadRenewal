using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Minecraft : Actor
    {
        [SerializeField] private GameObject _lavaPivot;

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(5, GetAttackDir(), 1.5f, 0); });
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("SummonSlave", RpcTarget.All);
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
        public void SummonSlave()
        {
            var newSlave = Global.PoolingManager.LocalSpawn("Minecraft_Slave", _lavaPivot.transform.position,
                Quaternion.identity, true);

            newSlave.GetComponent<Minecraft_Slave>().SetInfo(this.photonView, this.gameObject, GetAttackDir());
        }

        [PunRPC]
        public void LavaElemant()
        {
            var newLavaBasket = Global.PoolingManager.LocalSpawn("Minecraft_LavaBasket", _lavaPivot.transform.position,
                Quaternion.identity, true);
            var newLava = Global.PoolingManager.LocalSpawn("Minecraft_Lava", _lavaPivot.transform.position,
                Quaternion.identity, true);

            newLava.GetComponent<Minecraft_Lava>()
                .SetInfo(this.photonView, null, _lavaPivot.transform.position, GetAttackDir());
        }
    }
}
