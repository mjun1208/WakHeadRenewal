using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class BattleGround_Throw_Bomb : ActorSub
    {
        private BattleGround.ThrowType _throwType;

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir,
            BattleGround.ThrowType throwType, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _throwType = throwType;

            if (_throwType == BattleGround.ThrowType.MOLOTOV)
            {
                pos += new Vector3(0, 1.2f, 0);
            }

            this.transform.position = pos;
        }

        private void OnDamage()
        {
            if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
            {
                return;
            }

            switch (_throwType)
            {
                case BattleGround.ThrowType.GRENADE:
                {
                    _attackRange.Attack(targetEntity =>
                    {
                        var dir = targetEntity.transform.position - this.transform.position;
                        dir.Normalize();

                        targetEntity.KnockBack(20, dir, 1f, 0, AttackType.Actor, MyTeam,
                            "BattleGroundSkill_1_BoomEffect", _dir.x * 0.01f); }, MyTeam);
                    break;
                }
                case BattleGround.ThrowType.MOLOTOV:
                {
                    _attackRange.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, 10, AttackType.Actor, MyTeam,
                        "BattleGroundSkill_1_FireEffect", _dir.x * 0.01f); }, MyTeam);
                    break;
                }
                case BattleGround.ThrowType.FLASH_BANG:
                {
                    _attackRange.Attack(targetEntity =>
                    {
                        targetEntity.Stun(2.5f);
                        targetEntity.Damaged(targetEntity.transform.position, 10, AttackType.Actor, MyTeam);
                    }, MyTeam);
                    break;
                }
            }
        }
    }
}