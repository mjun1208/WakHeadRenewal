using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Tree_Leaf : ActorSub
    {
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntiy => { OnDamage(targetEntiy, 3); }, MyTeam, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 3);
            }, MyTeam, true);
        }

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, float scale, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = pos;
            this.transform.localScale = _originalScale * scale;
            
            _moveSpeed = Constant.TREE_LEFT_MOVE_SPEED;
            
            StartCoroutine(Go());
            
            _dir = Vector3.down;
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            StopAllCoroutines();

            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam);
            }

            Destroy();
        }

        protected override IEnumerator Go()
        {
            float goTime = 0;

            while (goTime < _lifeTime)
            {
                goTime += Time.deltaTime;
                _rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

                yield return null;
            }

            Destroy();
        }
    }
}
