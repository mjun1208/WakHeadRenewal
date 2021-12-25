using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Kakashi_Bomb : ActorSub
    {
        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = pos;
        }

        private void OnDamage()
        {
            if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.Damaged(this.transform.position, 8, AttackType.Actor, MyTeam,
                "BattleGroundSkill_1_BoomEffect", _dir.x * 0.01f); }, MyTeam);
        }
    }
}