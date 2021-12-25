using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Ahri_Heart : ActorSub
    {
        private List<GameObject> _collidedObjectList = new List<GameObject>();
        
        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _moveSpeed = Constant.AHRI_HEART_MOVE_SPEED;
            _collidedObjectList.Clear();

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntity => 
            {
                if (!_collidedObjectList.Contains(targetEntity.gameObject))
                {
                    OnDamage(targetEntity, 10);
                    _collidedObjectList.Add(targetEntity.gameObject);
                }
            }, MyTeam);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (!_collidedObjectList.Contains(targetSummoned.gameObject))
                {
                    if (_ownerPhotonView.IsMine)
                    {
                        targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                        _collidedObjectList.Add(targetSummoned.gameObject);
                    }
                    OnDamage(null, 10);
                }
            },MyTeam);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam, "HeartHitEffect");
                entity?.Heart(MyTeam);
            }
        }
    }
}