using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Summoned : ActorSub
    {
        public Action DeadAction;

        private int _maxHP;
        private int _currentHP;

        private bool _isDead = false;

        public bool IsDead
        {
            get { return _isDead; }
            protected set
            {
                if (value)
                {
                    DeadAction?.Invoke();
                }

                _isDead = value;
            }
        }

        public int MaxHP
        {
            get { return _maxHP; }
            protected set { _maxHP = value; }
        }

        public int HP
        {
            get { return _currentHP; }
            protected set { _currentHP = value; }
        }

        protected virtual void Awake()
        {
            DeadAction += Dead;
        }

        protected virtual void Dead()
        {

        }

        public void Damaged(Vector3 pos, string effectName = "HitEffect", bool effectFlip = false)
        {
            if (photonView == null)
            {
                return;
            }

            photonView.RPC("OnDamageRPC", RpcTarget.All, pos, effectName, effectFlip);
        }

        [PunRPC]
        public void OnDamageRPC(Vector3 pos, string effectName, bool effectFlip)
        {
            OnDamage(pos, effectName, effectFlip);
        }

        public void OnDamage(Vector3 pos, string effectName, bool effectFlip)
        {
            var randomPos = (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f;

            Global.PoolingManager.LocalSpawn("HitEffect", this.transform.position + randomPos,
                Quaternion.Euler(new Vector3(0, effectFlip ? 0 : -180, 0)), true);

            if (photonView.IsMine)
            {
                _currentHP--;

                if (_currentHP <= 0)
                {
                    IsDead = true;
                }
            }
        }
    }
}