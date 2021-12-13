using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class SimpleActor : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private List<RuntimeAnimatorController> _animatorControllerList;

        public void Select(int index)
        {
            _animator.runtimeAnimatorController = _animatorControllerList[index];
        }

        public void Confirmed()
        {
            _animator.Play("Attack");
        }

        public void Active_Attack()
        {
        }

        public void AttackLoop()
        {
            
        }

        public void PlayAttackSound()
        {
            
        }
        
        public void PlaySkill_1StartSound()
        {
            
        }
        
        public void PlaySkill_1Sound()
        {
            
        }

        public void PlaySkill_2StartSound()
        {
            
        }
        
        public void PlaySkill_2Sound()
        {
            
        }
    }
}
