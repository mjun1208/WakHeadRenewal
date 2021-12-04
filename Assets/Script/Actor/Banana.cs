using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana : Actor
    {
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0, MyTeam,
                "NormalAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

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

            photonView.RPC("SpawnBall", RpcTarget.All);
        }

        [PunRPC]
        public void SpawnBall()
        {
            var newBullet =
                Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

            newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }
    }
}