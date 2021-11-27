using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Vengenpro : Actor
    {
        private ObscuredFloat _attackPressTime = 0f;
        private ObscuredFloat _attackPressFullChargingTime = 0f;
        private ObscuredFloat _attackPressDelay = 0f;

        protected override void Update()
        {
            base.Update();

            if (_isAttack && _attackPressDelay <= 0f)
            {
                _attackPressDelay = 0f;

                if (_attackPressTime < 10f)
                {
                    _attackPressTime += Time.deltaTime;
                    _animator.SetFloat("AttackSpeed", 1 + _attackPressTime * 0.5f);
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

                targetEntity.KnockBack(10, dir.normalized, 3f, 1.5f);
            });
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
    }
}