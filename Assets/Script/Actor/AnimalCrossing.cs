﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class AnimalCrossing : Actor
    {
        [SerializeField] private AnimalCrossing_Fish myFish;

        private ObscuredBool _isCasting = false;
        private ObscuredBool _isBite = false;
        private IEnumerator _castingCoroutine = null;
        private ObscuredBool _isCastingComplete = false;

        private ObscuredBool _isHaveFish = false;

        protected override void Update()
        {
            base.Update();

            if (_isMoveInput || _isSkill_2Input || _isAttackInput)
            {
                Skill_1Cancel();
            }

            if (_isSkill_1Input && _isCasting && _isBite)
            {
                OnSkill_1();
            }
        }

        protected override void ForceStop()
        {
            base.ForceStop();

            if (_castingCoroutine != null)
            {
                StopCoroutine(_castingCoroutine);
            }

            _castingCoroutine = null;

            _isCasting = false;
            _isBite = false;
            _isCastingComplete = false;
        }

        public override void PlayAttackSound()
        {
            Global.SoundManager.Play("AnimalCrossing_Attack_Sound", this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            Global.SoundManager.Play("AnimalCrossing_Skill_1_Sound", this.transform.position);
        }
        
        public void PlaySkill_1_2Sound()
        {
            Global.SoundManager.Play("AnimalCrossing_Skill_1_2_Sound", this.transform.position);
        }

        public override void PlaySkill_2Sound()
        {
            Global.SoundManager.Play("AnimalCrossing_Skill_2_Sound", this.transform.position);
        }
        
        public void PlayGetButterFlySound()
        {
            Global.SoundManager.Play("배추흰나비", this.transform.position);
        }

        public void PlayGetFishSound()
        {
            if (myFish.GetMyFishIndex() != 5)
            {
                Global.SoundManager.Play($"Fish_{myFish.GetMyFishIndex()}_Sound", this.transform.position);
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            // 30퍼
            if (Random.Range(0, 10) < 3 && !_isHaveFish)
            {
                myFish.SelectFish(5);
                photonView.RPC("SetFish", RpcTarget.All, myFish.GetMyFishIndex());

                _isHaveFish = true;

                PlayGetButterFlySound();
                Global.PoolingManager.SpawnNotifyText("배추흰나비를 찾았다!!", Color.white);
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(5, GetAttackDir(), 0.5f, 0, AttackType.Actor, MyTeam,
                "AnimalCrossingAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ShootFish", RpcTarget.All, myFish.GetMyFishIndex());
        }

        public override void OnSkill_1()
        {
            if (!_isCasting)
            {
                // 캐스팅
                IsDoingSkill = true;
                IsSkill_1 = true;
                _isCasting = true;

                _animator.SetBool("IsSkill_1", true);

                _castingCoroutine = Casting();
                StartCoroutine(_castingCoroutine);
            }
            else
            {
                if (_isBite)
                {
                    _isCasting = false;
                    _isCastingComplete = false;
                    // 물고기가 물었을 때

                    _animator.SetBool("IsSkill_1_2", true);
                    _animator.SetBool("IsSkill_1_1", false);

                    if (OnSkillCoroutine == null)
                    {
                        OnSkillCoroutine = Fishing();
                        StartCoroutine(OnSkillCoroutine);
                    }
                }
            }
        }

        private void Skill_1Cancel()
        {
            if (IsDoingSkill && _isCastingComplete)
            {
                _isCasting = false;
                _isBite = false;

                if (_castingCoroutine != null)
                {
                    StopCoroutine(_castingCoroutine);
                    _castingCoroutine = null;
                }

                _isCastingComplete = false;

                photonView.RPC("SetActiveFish", RpcTarget.All, false);
                photonView.RPC("ResetAnimation", RpcTarget.All);

                SkillCancel();

                _animator.SetBool("IsSkill_1_1", false);
                _animator.SetBool("IsSkill_1_2", false);
            }
        }

        private IEnumerator Casting()
        {
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_1"))
            {
                yield return null;
            }

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }

            _isCastingComplete = true;

            yield return new WaitForSeconds(Random.Range(0.1f, 3f));

            _animator.SetBool("IsSkill_1_1", true);
            _animator.SetBool("IsSkill_1", false);

            _isBite = true;

            photonView.RPC("SetActiveFish", RpcTarget.All, false);

            _castingCoroutine = null;
        }

        private IEnumerator Fishing()
        {
            IsDoingSkill = true;

            myFish.SelectRandomFish();
            photonView.RPC("SetFish", RpcTarget.All, myFish.GetMyFishIndex());

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_1_2"))
            {
                yield return null;
            }

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }

            // end

            _isHaveFish = true;

            _animator.SetBool("IsSkill_1_2", false);

            IsDoingSkill = false;

            _isBite = false;

            photonView.RPC("SetActiveFish", RpcTarget.All, false);

            OnSkillCoroutine = null;
        }

        public override void OnSkill_2()
        {
            if (_isHaveFish)
            {
                base.OnSkill_2();
                _isHaveFish = false;
            }
            else
            {
                Global.PoolingManager.SpawnNotifyText("낚은 물고기가 없습니다.!!");
            }
        }

        [PunRPC]
        private void SetActiveFish(bool isActive)
        {
            myFish.SetActive(isActive);
        }

        [PunRPC]
        private void SetFish(int index)
        {
            myFish.SelectFish(index);
        }

        [PunRPC]
        private void ResetAnimation()
        {
            _animator.Rebind();
        }

        [PunRPC]
        private void ShootFish(int index)
        {
            var newFish = Global.PoolingManager.LocalSpawn("AnimalCrossing_FishBullet", this.transform.position,
                Quaternion.identity, true);

            newFish.GetComponent<AnimalCrossing_FishBullet>()
                .SetInfo(this.photonView, this.gameObject, GetAttackDir(), index, MyTeam);
        }

        protected override void Dead()
        {
            base.Dead();

            _isHaveFish = false;
            _isCasting = false;
            _isBite = false;
        }
    }
}