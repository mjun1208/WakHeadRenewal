using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using WakHead;

namespace WakHead
{
    public class Ranger_Titan : ActorSub
    {
        private Vector3 _originalScale;

        private readonly Vector3 offset = new Vector3(5.5f, 3f);

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }
        
        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position += new Vector3(offset.x * dir.x, offset.y); 
            
            float rotationScale = _originalScale.x * -dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
        }

        public void ActiveDamage()
        {
            _attackRange.Attack(targetEntity => { OnDamage(targetEntity, 30); }, MyTeam);
        }
        
        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                var dir = entity.transform.position - this.transform.position;
                dir.Normalize();

                entity?.KnockBack(damage, dir,0.5f, 1f, MyTeam, 
                    "RangerSkill_1Effect", dir.x * 0.1f, dir.x > 0f);
            }
        }
    }
}
