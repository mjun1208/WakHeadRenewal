﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Ranger : Actor
    {
        [SerializeField] private GameObject _attackBeam;
        private float _rollingGauge = 0f;
        
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.Damaged( targetEntity.transform.position, 3, MyTeam, 
                "RangerAttackEffect", 0 , GetAttackDir().x > 0); }, MyTeam);
        }
        
        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            photonView.RPC("SpawnTitan", RpcTarget.All);
        }
        
        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("SpawnRolling", RpcTarget.All, _rollingGauge);
        }

        protected override void Attack()
        {
            base.Attack();

            if (!_isAttackInput)
            {
                _animator.SetBool("IsAttackLoop", _isAttackInput);
            }

            if (_attackBeam.activeSelf != _isAttackInput)
            {
                photonView.RPC("SetEnableBeam", RpcTarget.All, _isAttackInput.GetDecrypted());
            }
        }

        protected override void CheckAttack()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AttackLoop"))
            {
                _isAttack = true;
            }
            else
            {
                _isAttack = false;
            }
        }

        public void AttackLoop()
        {
            _animator.SetBool("IsAttackLoop", true);
        }
        

        [PunRPC]
        public void SetEnableBeam(bool enable)
        {
            _attackBeam.SetActive(enable);
        }

        [PunRPC]
        public void SpawnTitan()
        {
            var newTitan = Global.PoolingManager.LocalSpawn("Ranger_Titan", this.transform.position, Quaternion.identity, true);

            newTitan.GetComponent<Ranger_Titan>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }
        
        [PunRPC]
        public void SpawnRolling(float gauge)
        {
            var newTitan = Global.PoolingManager.LocalSpawn("Ranger_Rolling", this.transform.position, Quaternion.identity, true);

            newTitan.GetComponent<Ranger_Rolling>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), gauge, MyTeam);
        }

        protected override void Dead()
        {
            base.Dead();
            
            photonView.RPC("SetEnableBeam", RpcTarget.All, false);
        }
    }
}
