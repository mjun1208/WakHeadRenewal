using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

namespace WakHead
{
    public class Tree : Actor
    {
        private Vector3 _realOriginalScale;

        private int _attackSoundIndex = 0;
        
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
            
            var scale = _originalScale.y / _realOriginalScale.y;
            
            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(5 + (int)Math.Round(3f * scale), GetAttackDir(), 0.5f, 0, AttackType.Actor, MyTeam,
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

            if ( HP + ((MaxHP - HP) * 0.2f) > MaxHP)
            {
                HP = MaxHP;
            }
            else
            {
                HP += (int)Mathf.Round(((MaxHP - HP) * 0.2f));
            }
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            for (int i = 0; i < 10; i++)
            {
                var randomPos = (Vector3) Random.insideUnitCircle * 0.5f * this.transform.localScale.y;

                var leafScale = _originalScale.y / _realOriginalScale.y;

                photonView.RPC("ShakeLeaf", RpcTarget.All, randomPos.x, randomPos.y, leafScale);
            }
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();

            Global.SoundManager.Play($"Tree_Attack_Sound_{_attackSoundIndex}", this.transform.position);
            
            if (++_attackSoundIndex >= 6)
            {
                _attackSoundIndex = 0;
            }
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            
            Global.SoundManager.Play("Tree_Skill_1_Sound", this.transform.position);
        }
        
        public void PlaySkill_1_StartSound()
        {
            Global.SoundManager.Play("Tree_Skill_1_Start_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            
            Global.SoundManager.Play("Tree_Skill_2_Sound", this.transform.position);
        }

        [PunRPC]
        public void ShakeLeaf(float randomPos_x, float randomPos_y, float leaftScale)
        {
            var newLeaf =
                Global.PoolingManager.LocalSpawn("Tree_Leaf", this.transform.position + new Vector3(randomPos_x, randomPos_y), Quaternion.identity, true);

            newLeaf.GetComponent<Tree_Leaf>().SetInfo(this.photonView, this.gameObject,
                this.transform.position + new Vector3(randomPos_x, randomPos_y), GetAttackDir(), leaftScale, MyTeam);
        }
        
        protected override void Dead()
        {
            base.Dead();

            _originalScale = _realOriginalScale;
            this.transform.localScale = _originalScale;
        }
    }
}
