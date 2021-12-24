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

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(25, GetAttackDir(), 1.5f, 0, MyTeam); }, MyTeam);
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

            newSlave.GetComponent<Minecraft_Slave>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }

        [PunRPC]
        public void LavaElemant()
        {
            var newLavaBasket = Global.PoolingManager.LocalSpawn("Minecraft_LavaBasket", _lavaPivot.transform.position,
                Quaternion.identity, true);
            var newLava = Global.PoolingManager.LocalSpawn("Minecraft_Lava", _lavaPivot.transform.position,
                Quaternion.identity, true);

            newLava.GetComponent<Minecraft_Lava>()
                .SetInfo(this.photonView, null, _lavaPivot.transform.position, GetAttackDir(), MyTeam);
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();
            Global.SoundManager.Play("Minecraft_Attack_Sound", this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            Global.SoundManager.Play("Minecraft_Skill_1_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            Global.SoundManager.Play("Minecraft_Skill_2_Sound", this.transform.position);
        }
        
        public void PlaySkill_2_StartSound()
        {
            Global.SoundManager.Play("Minecraft_Skill_2_Start_Sound", this.transform.position);
        }
    }
}
