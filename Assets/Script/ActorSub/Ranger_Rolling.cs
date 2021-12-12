using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Ranger_Rolling : ActorSub
    {
        [SerializeField] private GameObject _rollingBeam;
        private Vector3 _originalScale;
        private bool _isAttack = false;

        private readonly float attackDelay = 0.1f;
        private float _attackTimer = 0f;

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }
        
        public  void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, float gauge, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);
            
            float rotationScale = _originalScale.x * dir.x;
            
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            
            _isAttack = false;

            _rollingBeam.transform.localScale = new Vector3(gauge, 1, 1);
        }

        public void ActiveAttack()
        {
            _isAttack = true;
        }

        private void Update()
        {
            if (_isAttack)
            {
                _attackTimer += Time.deltaTime;
                
                if (_attackTimer > attackDelay)
                {
                    _attackTimer = 0f;
                    _attackRange.Attack(targetEntity => { OnDamage(targetEntity, 8); }, MyTeam);
                }
            }
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam, "RangerSkill_2Effect", 0 , _dir.x > 0);
            }
        }
        
        protected virtual void OnDestory(ActorSub actorSub)
        {
            DestoryAction -= OnDestory;

            _isAttack = false;
        }
    }
}