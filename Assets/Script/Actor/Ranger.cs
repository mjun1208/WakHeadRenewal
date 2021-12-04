using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Ranger : Actor
    {
        [SerializeField] private GameObject _attackBeam;
        
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.Damaged( targetEntity.transform.position, 3, MyTeam, 
                "TreeSkill_2Effect"); }, MyTeam);
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
            
            photonView.RPC("SpawnRolling", RpcTarget.All);
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
        public void SpawnRolling()
        {
            var newTitan = Global.PoolingManager.LocalSpawn("Ranger_Rolling", this.transform.position, Quaternion.identity, true);

            newTitan.GetComponent<Ranger_Rolling>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }

        protected override void Dead()
        {
            base.Dead();
            
            photonView.RPC("SetEnableBeam", RpcTarget.All, false);
        }
    }
}
