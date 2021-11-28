using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Naruto_Dummy : Summoned, IPunObservable
    {
        [SerializeField] private Animator _animator;

        private Vector3 _originalScale;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_dir.x);
            }
            else
            {
                float rotationScale = _originalScale.x * (float) stream.ReceiveNext();
                this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            }
        }

        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            IsDead = false;

            _lifeTime = 0f;

            _animator.Rebind();

            MaxHP = 5;
            HP = MaxHP;
        }

        protected override void Awake()
        {
            base.Awake();

            _originalScale = this.transform.localScale;
        }

        private void Update()
        {
            _lifeTime += Time.deltaTime;

            if (_lifeTime > 10f)
            {
                IsDead = true;
            }
        }

        private void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(3, _dir, 1.5f, 0, MyTeam); });
        }

        public void SetDir(Vector3 dir)
        {
            _dir = dir;
            float rotationScale = _originalScale.x * dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
        }

        public void Attack(bool isAttack)
        {
            _animator.SetBool("IsAttack", isAttack);
        }

        public void SetAnimationParameter(string name, bool isTrue)
        {
            _animator.SetBool(name, isTrue);
        }

        public void Charging()
        {
        }

        public void Rasengan()
        {
        }
    }
}