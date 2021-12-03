using System.Collections;
using System.Collections.Generic;
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
        
        protected override void Attack()
        {
            base.Attack();
            
            if (!_isAttackInput)
            {
                _animator.SetBool("IsAttackLoop", _isAttackInput);
            }
            
            _attackBeam.SetActive(_isAttackInput);
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
        
        protected override void Dead()
        {
            base.Dead();
            
            _attackBeam.SetActive(false);
        }
    }
}
