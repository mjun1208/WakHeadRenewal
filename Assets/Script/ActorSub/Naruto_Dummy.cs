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

            Play_Spawn_Sound();
            PlaySkill_1Sound();
        }

        private void Update()
        {
            _lifeTime += Time.deltaTime;

            if (_lifeTime > 15f)
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

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(8, _dir, 1.5f, 0, AttackType.Actor, MyTeam, 
                "NarutoAttackEffect", _dir.x * 0.1f, _dir.x > 0); }, MyTeam);
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
            SpawnChargingEffect();
        }
        
        public void SpawnChargingEffect()
        {
            var chargingEffect = Global.PoolingManager.LocalSpawn("NarutoChargingEffect", this.transform.position,
                Quaternion.identity, true);
        }

        public void PlayAttackSound()
        {
            // Global.SoundManager.Play("Naruto_Attack_Sound", this.transform.position);
        }
        
        public void PlaySkill_1Sound()
        {
            Global.SoundManager.Play("Naruto_Skill_1_Sound", this.transform.position);
        }
        
        public void Play_Spawn_Sound()
        {
            Global.SoundManager.Play("Naruto_Dummy_Spawn_Sound", this.transform.position);
        }

        public void PlaySkill_2Sound()
        {
            // Global.SoundManager.Play("Naruto_Skill_2_Sound", this.transform.position);
        }
        
        public void PlaySkill_2_StartSound()
        {
            // Global.SoundManager.Play("Naruto_Skill_2_Start_Sound", this.transform.position);
        }
        
        public void PlayChargingRasenganSound()
        {
            // Global.SoundManager.Play("Naruto_Rasengan_Charging", this.transform.position);
        }
        
        public void Rasengan()
        {
        }
    }
}