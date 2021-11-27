using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class BattleGround_Throw : ActorSub
    {
        private BattleGround.ThrowType _throwType;

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, BattleGround.ThrowType throwType, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _throwType = throwType;

            _rigid.velocity = Vector2.zero;

            _rigid.AddForce(new Vector2(0f, 6f), ForceMode2D.Impulse);

            _moveSpeed = Constant.BATTLEGROUND_THROW_MOVE_SPEED;
            _lifeTime = Constant.BATTLEGROUND_THROW_LIFETIME;

            StartCoroutine(Go());
        }

        private void Update()
        {
            this.transform.Rotate(new Vector3(0, 0, 720f) * Time.deltaTime);

            _attackRange.AttackEntity(targetEntity => { OnDamage(targetEntity, 5); }, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 5);
            }, true);
        }

        protected override IEnumerator Go()
        {
            float goTime = 0;

            while (goTime < _lifeTime)
            {
                goTime += Time.deltaTime;

                this.transform.position += _dir * _moveSpeed * Time.deltaTime;
                // this.transform.Translate(_dir * _moveSpeed * Time.deltaTime);
                //_rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

                yield return null;
            }

            Destroy();
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            StopAllCoroutines();

            if (_ownerPhotonView.IsMine)
            {
                entity?.KnockBack(damage, _dir, 1f, 0f);
            }

            Destroy();
        }

        protected override void OnDestory(ActorSub actorSub)
        {
            base.OnDestory(actorSub);

            if (_ownerPhotonView == null || !_ownerPhotonView.IsMine)
            {
                return;
            }

            var newBomb = Global.PoolingManager.LocalSpawn(
                $"BattleGround_Throw_{(BattleGround.ThrowType) _throwType}_Bomb", this.transform.position,
                Quaternion.identity, false);
            newBomb.GetComponent<BattleGround_Throw_Bomb>()
                .SetInfo(_ownerPhotonView, _owner, this.transform.position, _dir, _throwType, MyTeam);
        }

        public override void Destroy()
        {
            DestoryAction?.Invoke(this);
        }
    }
}