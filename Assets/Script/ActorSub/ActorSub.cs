using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class ActorSub : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected Rigidbody2D _rigid;
        [SerializeField] protected AttackRange _attackRange;

        protected GameObject _owner;
        protected Vector3 _dir;
        protected PhotonView _ownerPhotonView = null;

        protected float _moveSpeed = Constant.ACTOR_SUB_DEFAULT_MOVE_SPEED;
        protected float _lifeTime = Constant.ACTOR_SUB_DEFAULT_LIFETIME;

        public Action<ActorSub> DestoryAction = null;

        public Team MyTeam { get; protected set; } = Team.None;

        public virtual void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            MyTeam = team;
            
            _ownerPhotonView = ownerPhotonView;

            if (owner != null)
            {
                if (_attackRange != null)
                {
                    _attackRange.SetOwner(owner);
                    _attackRange.SetTeam(MyTeam);
                    _attackRange.CollidedObjectList.Clear();
                    _attackRange.CollidedSummonedObjectList.Clear();
                }
                this.transform.position = owner.transform.position;

                _owner = owner;
            }

            _dir = dir;

            DestoryAction += OnDestory;
        }

        protected virtual void OnDamage(Entity entity, int damage)
        {
            StopAllCoroutines();

            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam);
            }

            Destroy();
        }

        protected virtual IEnumerator Go()
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

        public virtual void Destroy()
        {
            DestoryAction?.Invoke(this);

            Global.PoolingManager.LocalDespawn(this.gameObject);
        }

        protected virtual void OnDestory(ActorSub actorSub)
        {
            DestoryAction -= OnDestory;
        }
    }
}