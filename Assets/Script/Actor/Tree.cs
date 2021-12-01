using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WakHead
{
    public class Tree : Actor
    {
        private Vector3 _realOriginalScale;
        
        protected override void Awake()
        {
            base.Awake();

            _realOriginalScale = _originalScale;
        }
        
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0, MyTeam,
                "TreeAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _originalScale *= 1.2f;
            this.transform.localScale *= 1.2f;
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }
        
        protected override void Dead()
        {
            base.Dead();

            _originalScale = _realOriginalScale;
            this.transform.localScale = _originalScale;
        }
    }
}
