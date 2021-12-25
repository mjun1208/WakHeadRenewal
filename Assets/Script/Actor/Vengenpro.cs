using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Vengenpro : Actor
    {
        [SerializeField] private AudioSource _attackSound;
        
        private float _soundSpeed = 1f; 
        
        private ObscuredFloat _attackPressTime = 0f;
        private ObscuredFloat _attackPressFullChargingTime = 0f;
        private ObscuredFloat _attackPressDelay = 0f;

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(_soundSpeed);
            }
            else
            {
                _soundSpeed = (float) stream.ReceiveNext();
            }
        }

        protected override void Update()
        {
            base.Update();
            
            _attackSound.pitch = _soundSpeed;
            
            if (!photonView.IsMine)
            {
                return;
            }
            
            if (_isAttack && _attackPressDelay <= 0f)
            {
                _attackPressDelay = 0f;

                if (_attackPressTime < 10f)
                {
                    _attackPressTime += Time.deltaTime;
                    _animator.SetFloat("AttackSpeed", 1 + _attackPressTime * 0.5f);
                    _soundSpeed = 1 + _attackPressTime * 0.1f;
                }
                else
                {
                    _attackPressFullChargingTime += Time.deltaTime;
                }

                if (_attackPressFullChargingTime >= 1.5f)
                {
                    _attackPressFullChargingTime = 0f;
                    _attackPressDelay = 0.5f;
                }
            }
            else
            {
                _attackPressDelay -= Time.deltaTime;

                _attackPressTime = 0;
                _animator.SetFloat("AttackSpeed", 1);
                _soundSpeed = 1f;
            }
        }
        
        protected override void CheckAttack()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (!_isAttack)
                {
                    photonView.RPC("PlayAttackSound", RpcTarget.All , true);
                }
                _isAttack = true;
            }
            else
            {
                if (_isAttack)
                {
                    photonView.RPC("PlayAttackSound", RpcTarget.All , false);
                }
                _isAttack = false;
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 randomDir = new Vector2(Random.Range(0f, 1f), Random.Range(-0.5f, 0.5f));

                randomDir.x *= GetAttackDir().x;

                photonView.RPC("ShootNote", RpcTarget.All, randomDir);
            }
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ShootZzang", RpcTarget.All);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_2Range.AttackEntity(targetEntity =>
            {
                var dir = targetEntity.transform.position - this.transform.position;

                targetEntity.KnockBack(10, dir.normalized, 3f, 1.5f, AttackType.Actor, MyTeam, 
                    "VengenproSkill_2Effect" , 0.01f, false); }, MyTeam);
        }

        [PunRPC]
        public void ShootNote(Vector2 randomDir)
        {
            var newNote =
                Global.PoolingManager.LocalSpawn("Vengenpro_Note", this.transform.position, Quaternion.identity, true);

            newNote.GetComponent<Vengenpro_Note>().SetInfo(this.photonView, this.gameObject, randomDir, MyTeam);
        }

        [PunRPC]
        public void ShootZzang()
        {
            var newZzang = Global.PoolingManager.LocalSpawn("Vengenpro_Zzang", this.transform.position,
                Quaternion.identity, true);

            newZzang.GetComponent<Vengenpro_Zzang>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }

        [PunRPC]
        public void SonicBoom()
        {
            var newSonic = Global.PoolingManager.LocalSpawn("Vengenpro_SonicBoom", this.transform.position,
                Quaternion.identity, true);
        }

        [PunRPC]
        public void PlayAttackSound(bool isAttack)
        {
            if (isAttack)
            {
                _attackSound.Play();
            }
            else
            {
                _attackSound.Stop();
            }
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            Global.SoundManager.Play("Vengenpro_Skill_1_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            Global.SoundManager.Play("Vengenpro_Skill_2_Sound", this.transform.position);
        }
    }
}