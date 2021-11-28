using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Ahri_Heart : ActorSub
    {
        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _moveSpeed = Constant.AHRI_HEART_MOVE_SPEED;

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntity => { OnDamage(targetEntity, 10); },MyTeam , true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 10);
            },MyTeam, true);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            StopAllCoroutines();

            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam);
                entity?.Heart(MyTeam);
            }

            Destroy();
        }
    }
}