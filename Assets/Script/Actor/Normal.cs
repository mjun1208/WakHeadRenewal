using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Normal : Actor
    {
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.8f, 0, MyTeam,
                "NormalAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_1Range.Attack(targetEntity => { targetEntity.Grab(15, this.transform.position, 10f, MyTeam,
                "NormalSkill_1Effect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ShootBullet", RpcTarget.All);
        }
        
        public override void PlayAttackSound()
        {
            Global.SoundManager.Play("Normal_Attack_Sound", this.transform.position);
        }
        
        public void PlaySkill_1StartSound()
        {
            Global.SoundManager.Play("Normal_Skill_1_Start_Sound", this.transform.position);
        }
        
        public override void PlaySkill_1Sound()
        {
            Global.SoundManager.Play("Normal_Skill_1_Sound", this.transform.position);
        }
        
        public void PlaySkill_2StartSound()
        {
            Global.SoundManager.Play("Normal_Skill_2_Start_Sound", this.transform.position);
        }
        
        public override void PlaySkill_2Sound()
        {
            Global.SoundManager.Play("Normal_Skill_2_Sound", this.transform.position);
        }
        
        [PunRPC]
        public void ShootBullet()
        {
            var newBullet =
                Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

            newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }
    }
}