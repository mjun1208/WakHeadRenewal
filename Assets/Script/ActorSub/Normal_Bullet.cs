using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Normal_Bullet : ActorSub
    {
        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _moveSpeed = Constant.NORMAL_BULLET_MOVE_SPEED;

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntity => { OnDamage(targetEntity, 30); }, MyTeam, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 30);
            }, MyTeam, true);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            StopAllCoroutines();

            if (_ownerPhotonView.IsMine)
            {
                entity?.KnockBack(damage, _dir, 3f, 0f, AttackType.Actor, MyTeam, "NormalSkill_2Effect" , -_dir.x * 0.3f, _dir.x > 0);
            }

            Destroy();
        }
    }
}