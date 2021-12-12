using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using Random = System.Random;

namespace WakHead
{
    public class BattleGround : Actor
    {
        public enum ThrowType
        {
            GRENADE,
            MOLOTOV,
            FLASH_BANG
        }

        private ThrowType _throwType;

        private Vector3 _throwPosition;
        private Vector3 _throwDir;

        private ObscuredBool _onSniping = false;

        private BattleGround_Aim _aim;
        private List<BattleGround_Throw> _throwList = new List<BattleGround_Throw>();

        protected override void ForceStop()
        {
            base.ForceStop();

            if (_aim != null)
            {
                _aim.Destroy();
                _aim = null;
            }
        }

        protected override void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (_aim != null)
            {
                if (Skill_1Input() || _isAttackInput)
                {
                    _aim.Destroy();

                    _isSkill_1Input = false;
                    _isAttackInput = false;

                    _aim = null;
                    
                    return;
                }
                
                if (Skill_2Input())
                {
                    if (_aim != null && _aim.gameObject.activeSelf && _aim.ShootCount > 0) 
                    {
                        _aim.Shoot(_aim.AimPosition);
                    }
                }
            }
            else if (_onSniping)
            {
                _onSniping = false;
                IsDoingSkill = false;
                IsSkill_2 = false;

                _animator.SetBool("IsSkill_2", false);
            }

            base.Update();
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(30, GetAttackDir(), 1f, 0, MyTeam,
                "NarutoAttackEffect", GetAttackDir().x * 0.1f, GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ThrowRPC", RpcTarget.All, (int) _throwType);
        }

        public override void OnSkill_1()
        {
            Skill_1_Delay = Skill_1_CoolTime;
            
            var throwValues = Enum.GetValues(typeof(ThrowType));
            _throwType = (ThrowType) throwValues.GetValue(new Random().Next(0, throwValues.Length));

            _animator.SetInteger("Throws", (int) _throwType);

            IsSkill_1 = true;

            if (OnSkillCoroutine == null)
            {
                IsDoingSkill = true;
                OnSkillCoroutine = OnSkill($"Skill_1_{(int) _throwType}");
                StartCoroutine(OnSkillCoroutine);

                _animator.SetBool($"IsSkill_1_{(int) _throwType}", true);
            }
        }

        public override void OnSkill_2()
        {
            _onSniping = true;

            IsDoingSkill = true;
            IsSkill_2 = true;

            _animator.SetBool("IsSkill_2", true);

            var centerPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            centerPosition.z = 0f;
            
            var newAim = Global.PoolingManager.Spawn("BattleGround_Aim", centerPosition, Quaternion.identity);
            _aim = newAim.GetComponent<BattleGround_Aim>();
            _aim.GetComponent<BattleGround_Aim>().SetInfo(this.photonView, this.gameObject, centerPosition, GetAttackDir(), MyTeam);
            _aim.DestoryAction += EndSnipe;
        }

        public void EndSnipe(ActorSub aim)
        {
            aim.DestoryAction -= EndSnipe;

            Skill_2_Delay = Skill_2_CoolTime;
            
            IsDoingSkill = false;
            IsSkill_2 = false;
            _animator.SetBool("IsSkill_2", false);

            aim = null;
        }

        [PunRPC]
        public void ThrowRPC(int throwType)
        {
            var newThrow = Global.PoolingManager.LocalSpawn($"BattleGround_Throw_{(ThrowType) throwType}",
                this.transform.position, Quaternion.identity, true);
            var newThrowScript = newThrow.GetComponent<BattleGround_Throw>();
            newThrowScript.SetInfo(this.photonView, this.gameObject, GetAttackDir(), (ThrowType) throwType, MyTeam);

            newThrowScript.DestoryAction += DespawnThrow;
            _throwList.Add(newThrowScript);
        }

        public void DespawnThrow(ActorSub throws)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            throws.DestoryAction -= DespawnThrow;

            photonView.RPC("DespawnThrowRPC", RpcTarget.All, GetThrowIndex(throws as BattleGround_Throw));
        }

        private int GetThrowIndex(BattleGround_Throw targetThrow)
        {
            int index = 0;

            foreach (var throws in _throwList)
            {
                if (throws == targetThrow)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        [PunRPC]
        public void DespawnThrowRPC(int count)
        {
            if (_throwList.Count > count)
            {
                var targetThrow = _throwList[count];

                if (targetThrow != null)
                {
                    _throwList.Remove(targetThrow);

                    Global.PoolingManager.LocalDespawn(targetThrow.gameObject);
                }
            }
        }

        protected override void Dead()
        {
            base.Dead();

            if (_aim != null)
            {
                _aim.Destroy();
                _aim = null;
            }
        }
    }
}