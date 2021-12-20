using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Ahri : Actor
    {
        private Ahri_Orb _myOrb;

        private ObscuredFloat _rushSpeed = 15f;

        protected override void Awake()
        {
            base.Awake();
            
            _myOrb = Global.PoolingManager.LocalSpawn("Ahri_Orb", this.transform.position, Quaternion.identity, true).GetComponent<Ahri_Orb>();
            _myOrb.gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            if (IsDoingSkill && IsSkill_2)
            {
                SpiritRush();
            }
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ShootHeart", RpcTarget.All);
        }

        protected override void Attack()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
                !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") &&
                !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }

            _animator.SetBool("IsAttack", _isAttackInput && !_myOrb.gameObject.activeSelf);
            
            if (_isAttackInput)
            {
                if (_myOrb != null && !_myOrb.gameObject.activeSelf)
                {
                    photonView.RPC("ShootOrb", RpcTarget.All, GetAttackDir().x);
                }
            }
        }

        private void SpiritRush()
        {
            _rigid.MovePosition(transform.position + GetAttackDir() * _rushSpeed * Time.deltaTime);
        }
        
        public override void PlayAttackSound()
        {
            base.PlayAttackSound();
            
            Global.SoundManager.Play("Ahri_Attack_Sound" , this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            
            Global.SoundManager.Play("Ahri_Skill_1_Sound" , this.transform.position);
        }
        
        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            
            Global.SoundManager.Play("Ahri_Skill_2_Sound" , this.transform.position);
        }


        [PunRPC]
        public void ShootOrb(float dir_x)
        {
            base.Attack();
            _myOrb.gameObject.SetActive(true);
            _myOrb.GetComponent<Ahri_Orb>().SetInfo(this.photonView, this.gameObject, new Vector3(dir_x, 0, 0), MyTeam);
        }

        [PunRPC]
        public void ShootHeart()
        {
            var newHeart =
                Global.PoolingManager.LocalSpawn("Ahri_Heart", this.transform.position, Quaternion.identity, true);

            newHeart.GetComponent<Ahri_Heart>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }

        private void OnDestroy()
        {
            Global.PoolingManager.LocalDespawn(_myOrb.gameObject);
        }
    }
}