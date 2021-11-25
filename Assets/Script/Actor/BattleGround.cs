﻿using Photon.Pun;
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
                if (Skill_2Input())
                {
                    photonView.RPC("SnipeShootRPC", RpcTarget.All, _aim.AimPosition);
                }
            }

            base.Update();
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0); });
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
            photonView.RPC("SetAimInfoRPC", RpcTarget.All, centerPosition, newAim.GetPhotonView().ViewID);
        }

        public void EndSnipe(ActorSub aim)
        {
            aim.DestoryAction -= EndSnipe;

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
            newThrowScript.SetInfo(this.photonView, this.gameObject, GetAttackDir(), (ThrowType) throwType);

            newThrowScript.DestoryAction += DespawnThrow;
            _throwList.Add(newThrowScript);
        }

        [PunRPC]
        public void SetAimInfoRPC(Vector3 aimPosition, int photonViewID)
        {
            var newAim = PhotonView.Find(photonViewID).gameObject.GetComponent<BattleGround_Aim>();

            newAim.GetComponent<BattleGround_Aim>().SetInfo(this.photonView, this.gameObject, GetAttackDir());

            newAim.transform.position = aimPosition;

            _aim = newAim.GetComponent<BattleGround_Aim>();
            _aim.DestoryAction += EndSnipe;
        }

        [PunRPC]
        public void SnipeShootRPC(Vector3 aimPosition)
        {
            if (_aim != null)
            {
                _aim.Shoot(aimPosition);
            }
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