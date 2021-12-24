using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;

namespace WakHead
{
    public class VR : Actor
    {
        [SerializeField] private GameObject _light;
        private bool _isInvisibility = false;
        private bool _isAssassin = false;

        protected override void Update()
        {
            base.Update();

            if (!IsSkill_1)
            {
                _isAssassin = false;
            }
        }

        protected override void ForceStop()
        {
            base.ForceStop();

            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }

        protected override void Active_Attack()
        {
            _light.gameObject.SetActive(true);

            _light.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            _light.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.3f).SetEase(Ease.Flash)
                .OnComplete(() => { _light.gameObject.SetActive(false); });

            if (!photonView.IsMine)
            {
                return;
            }
            
            _attackRange.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, _isAssassin ? 8 : 5, MyTeam,
                "VRAttackEffect" , -GetAttackDir().x * 0.25f , GetAttackDir().x < 0); }, MyTeam);

            _isAssassin = false;
        }

        protected override void Active_Skill_1()
        {
            PlaySkill_1Sound();
            
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_1Range.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, _isAssassin ? 6 : 3, MyTeam,
                Random.Range(0, 2) == 0 ? "VRSkill_1Effect_1" : "VRSkill_1Effect_2", 0, Random.Range(0, 2) == 0); }, MyTeam);
            _skill_1Range.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, _isAssassin ? 6 : 3, MyTeam,
                Random.Range(0, 2) == 0 ? "VRSkill_1Effect_1" : "VRSkill_1Effect_2", 0, Random.Range(0, 2) == 0); }, MyTeam);
        }

        protected override void Attack()
        {
            base.Attack();

            if (_isAttackInput)
            {
                if (_isInvisibility)
                {
                    _isAssassin = true;
                }

                photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
            }
        }

        public override void OnSkill_1()
        {
            base.OnSkill_1();

            Skill_1_Delay = Skill_1_CoolTime;
            
            if (_isInvisibility)
            {
                _isAssassin = true;
            }
            
            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }

        public override void OnSkill_2()
        {
            IsSkill_2 = true;
            IsDoingSkill = true;
            
            Skill_2_Delay = Skill_2_CoolTime;
            
            photonView.RPC("InvisibilityRPC", RpcTarget.All);
        }

        protected override void OnDamage(bool isChimpanzee)
        {
            base.OnDamage(isChimpanzee);
            
            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }

        [PunRPC]
        public void InvisibilityRPC()
        {
            PlaySkill_2Sound();
            
            _isInvisibility = true;

            _animator.Rebind();

            float targetAlpha = photonView.IsMine ? 0.5f : 0f;

            _renderer.DOColor(new Color(1, 1, 1, targetAlpha), 0.8f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                IsSkill_2 = false;
                IsDoingSkill = false;
            });
        }

        [PunRPC]
        public void DisInvisibilityRPC()
        {
            if (_isInvisibility)
            {
                PlaySkill_2_ShowSound();

                _isInvisibility = false;

                _renderer.color = new Color(1, 1, 1, 1);
            }
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();
            Global.SoundManager.Play("VR_Attack_Sound" , this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            Global.SoundManager.Play("VR_Skill_1_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            Global.SoundManager.Play("VR_Skill_2_Sound" , this.transform.position);
        }
        
        public void PlaySkill_2_ShowSound()
        {
            Global.SoundManager.Play("VR_Skill_2_Show_Sound" , this.transform.position);
        }
    }
}