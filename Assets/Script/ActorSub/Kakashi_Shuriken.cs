using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Kakashi_Shuriken : ActorSub
    {
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 position, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);
            this.transform.position = position;

            _moveSpeed = Constant.KAKASHI_SHURIKEN_MOVE_SPEED;

            float rotationScale = _originalScale.x * dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntiy => { OnDamage(targetEntiy, 3); }, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 3);
            }, true);
        }

        public override void Destroy()
        {
            DestoryAction?.Invoke(this);
        }

        protected override void OnDestory(ActorSub actorSub)
        {
            base.OnDestory(actorSub);

            if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
            {
                return;
            }

            var newBomb =
                Global.PoolingManager.LocalSpawn("Kakashi_Bomb", this.transform.position, Quaternion.identity, false);
            newBomb.GetComponent<Kakashi_Bomb>().SetInfo(_ownerPhotonView, _owner, this.transform.position, _dir, MyTeam);
        }
    }
}