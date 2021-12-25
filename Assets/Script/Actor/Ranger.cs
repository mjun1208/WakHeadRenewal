using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class Ranger : Actor
    {
        [SerializeField] private GameObject _attackBeam;
        [SerializeField] private GameObject _chargingGaugeObject;
        [SerializeField] private Image _chargingGaugeImage;
        [SerializeField] private Image _chargingOverGaugeImage;
        private float _rollingGauge = 0f;

        protected override void ForceStop()
        {
            base.ForceStop();
            
            _chargingGaugeObject.SetActive(false);
            SetEnableBeam(false);
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { 
                targetEntity.Damaged( targetEntity.transform.position, 3, MyTeam, 
                "RangerAttackEffect", 0 , GetAttackDir().x > 0);

                _chargingGaugeObject.SetActive(true);

                if (_rollingGauge <= 2f)
                {
                    _rollingGauge += 0.05f;
                }

                GaugeUpdate();
            }, MyTeam);
        }

        public void GaugeUpdate()
        {
            if (_rollingGauge < 1f)
            {
                _chargingGaugeImage.fillAmount = _rollingGauge;
                _chargingOverGaugeImage.fillAmount = 0f;
            }
            else
            {
                _chargingGaugeImage.fillAmount = 1f;
                _chargingOverGaugeImage.fillAmount = _rollingGauge - 1f;
            }
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
                _chargingGaugeObject.SetActive(false);
                _animator.SetBool("IsAttackLoop", _isAttackInput);
            }

            _chargingGaugeObject.transform.localScale = new Vector3(GetAttackDir().x, 1, 1);
            
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
            
            _rollingGauge = 0f;
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();

            Global.SoundManager.Play("Ranger_Attack_Sound", this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            
            Global.SoundManager.Play("Ranger_Skill_1_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            
            Global.SoundManager.Play("Ranger_Skill_2_Sound" , this.transform.position);
        }

        protected override void Dead()
        {
            base.Dead();
            
            photonView.RPC("SetEnableBeam", RpcTarget.All, false);
        }
    }
}
