using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Sans_Gaster_Blast : ActorSub
    {
        private Vector3 _originalScale;
        private GameObject _gaster;

        private readonly float attackDelay = 0.1f;
        private float _attackTimer = 0f;

        public void Awake()
        {
            _originalScale = this.transform.localScale;
        }

        public void SetInfo(PhotonView ownerPhotonView, GameObject gaster, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            _gaster = gaster;

            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = pos;

            float rotationScale = _originalScale.x * dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

            _attackTimer = 0f;
        }

        private void Update()
        {
            _attackTimer += Time.deltaTime;
        
            if (_attackTimer > attackDelay)
            {
                _attackRange.Attack(targetEntity => { OnDamage(targetEntity, 10); }, MyTeam);
                _attackTimer = 0f;
            }
        }

        public override void Destroy()
        {
            if (_gaster != null)
            {
                Global.PoolingManager.LocalDespawn(_gaster);
            }

            Global.PoolingManager.LocalSpawn("DeathEffect", this.transform.position, this.transform.rotation, true);

            Global.PoolingManager.LocalDespawn(this.gameObject);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam, "SansSkill_2Effect", 0 , _dir.x > 0);
            }
        }
    }
}