﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana_Trampoline : Summoned
    {
        [SerializeField] private Animator _animator;
        private float _liftTime = 0f;

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            transform.position = pos;

            _liftTime = 0f;
            
            MaxHP = 5;
            HP = MaxHP;
        }
        
        public void Use()
        {
            photonView.RPC("UseRPC", RpcTarget.All);
        }

        [PunRPC]
        public void UseRPC()
        {
            _animator.Play("Use");
        }

        private void Update()
        {
            _liftTime += Time.deltaTime;
            
            if (_liftTime > 10f)
            {
                _liftTime = 0f;
                IsDead = true;
            }   
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var targetEntity = other.transform.GetComponent<Entity>();
            
            if (targetEntity != null)
            {
                OnDamage(targetEntity, 1);
            }
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (photonView.IsMine)
            {
                var dir = entity.transform.position - this.transform.position;
                entity.KnockBack(damage, dir.normalized, 2f, 0, AttackType.Actor, MyTeam);
                
                if (HP <= 0)
                {
                    IsDead = true;
                }
            }
        }
    }
}
