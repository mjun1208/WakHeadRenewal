using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Naruto : Actor
    {
        private List<Naruto_Dummy> _dummyList = new List<Naruto_Dummy>();
        private ObscuredBool _isSkill_2KeyDown = false;

        private ObscuredInt _chargingGauge = 0;

        private enum RasenganState
        {
            Ready,
            Charging,
            Shoot
        }

        private RasenganState _rasenganState;

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(_chargingGauge.GetDecrypted());
            }
            else
            {
                _chargingGauge = (int) stream.ReceiveNext();
            }
        }

        protected override void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            base.Update();

            for (int i = 0; i < _dummyList.Count; i++)
            {
                if (_dummyList[i].IsDead)
                {
                    var dummy = _dummyList[i];

                    photonView.RPC("SummonSmokeRPC", RpcTarget.All, dummy.transform.position);

                    _dummyList.Remove(dummy);

                    PhotonNetwork.Destroy(dummy.gameObject);
                }
            }

            if (_isMove)
            {
                foreach (var dummy in _dummyList)
                {
                    dummy.SetDir(GetAttackDir());
                }
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(5, GetAttackDir(), 1.5f, 0, MyTeam,
                "NarutoAttackEffect", GetAttackDir().x * 0.1f, GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Attack()
        {
            base.Attack();
            SetDummyAnimation("IsAttack", _isAttackInput);
        }

        protected override bool Skill_2Input()
        {
            if (Input.GetKey(KeyCode.C))
            {
                _isSkill_2KeyDown = true;
            }
            else
            {
                _isSkill_2KeyDown = false;
            }

            return base.Skill_2Input();
        }

        public override void OnSkill_1()
        {
            if (_rasenganState == RasenganState.Shoot)
            {
                Global.PoolingManager.SpawnNotifyText("나선환이 차징 되어있으면 분신 소환을 못 합니다.!!");

                return;
            }

            if (_dummyList.Count >= 5)
            {
                Global.PoolingManager.SpawnNotifyText("분신은 5명까지 소환 가능합니다.!!");

                return;
            }

            var newDummy = Global.PoolingManager.Spawn("Naruto_Dummy", this.transform.position, Quaternion.identity);
            newDummy.GetComponent<Naruto_Dummy>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
            _dummyList.Add(newDummy.GetComponent<Naruto_Dummy>());

            photonView.RPC("SummonSmokeRPC", RpcTarget.All, this.transform.position);
        }

        public override void OnSkill_2()
        {
            switch (_rasenganState)
            {
                case RasenganState.Ready:
                {
                    _rasenganState = RasenganState.Charging;

                    OnSkill_2();
                    SetDummyAnimation("IsSkill_2", true);

                    break;
                }
                case RasenganState.Charging:
                {
                    _chargingGauge = 0;

                    IsSkill_2 = true;

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ChargingRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                        SetDummyAnimation("IsSkill_2", true);
                    }

                    if (IsSkill_2)
                    {
                        _rasenganState = RasenganState.Shoot;
                    }

                    break;
                }
                case RasenganState.Shoot:
                {
                    IsSkill_2 = true;

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ShootRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                        SetDummyAnimation("IsSkill_2", true);
                    }

                    if (IsSkill_2)
                    {
                        _rasenganState = RasenganState.Ready;
                    }

                    break;
                }
            }
        }

        public void Charging()
        {
            _chargingGauge += 1;
        }

        private IEnumerator ChargingRasengan()
        {
            IsDoingSkill = true;

            _animator.SetBool("IsCharging", true);
            SetDummyAnimation("IsCharging", true);

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_1"))
            {
                yield return null;
            }

            _animator.SetBool("IsSkill_2", false);
            SetDummyAnimation("IsSkill_2", false);

            yield return null;

            while (_isSkill_2KeyDown && _chargingGauge < 3)
            {
                yield return null;
            }

            IsDoingSkill = false;
            IsSkill_1 = false;
            IsSkill_2 = false;

            _animator.SetBool("IsCharging", false);
            _animator.SetBool("IsCharged", true);

            SetDummyAnimation("IsCharging", false);
            SetDummyAnimation("IsCharged", true);

            OnSkillCoroutine = null;
        }

        private IEnumerator ShootRasengan()
        {
            IsDoingSkill = true;

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_2"))
            {
                yield return null;
            }

            SetDummyAnimation("IsSkill_2", false);
            SetDummyAnimation("IsCharged", false);

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }

            IsDoingSkill = false;
            IsSkill_1 = false;
            IsSkill_2 = false;

            _animator.SetBool("IsSkill_2", false);
            _animator.SetBool("IsCharged", false);

            SetDummyAnimation("IsSkill_2", false);
            SetDummyAnimation("IsCharged", false);

            OnSkillCoroutine = null;
        }

        public void Rasengan()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            foreach (var dummy in _dummyList)
            {
                photonView.RPC("DummyRasenganRPC", RpcTarget.All, dummy.transform.position);
            }

            photonView.RPC("RasenganRPC", RpcTarget.All);
        }

        [PunRPC]
        public void RasenganRPC()
        {
            var newRasengan = Global.PoolingManager.LocalSpawn("Naruto_Rasengan", this.transform.position,
                Quaternion.identity, true);
            newRasengan.GetComponent<Naruto_Rasengan>().SetInfo(this.photonView, this.gameObject,
                this.transform.position, GetAttackDir(), _chargingGauge, MyTeam);
        }

        [PunRPC]
        public void DummyRasenganRPC(Vector3 pos)
        {
            var newDummyRasengan = Global.PoolingManager.LocalSpawn("Naruto_Rasengan", pos, Quaternion.identity, true);
            newDummyRasengan.GetComponent<Naruto_Rasengan>()
                .SetInfo(this.photonView, this.gameObject, pos, GetAttackDir(), _chargingGauge, MyTeam);
        }

        [PunRPC]
        public void SummonSmokeRPC(Vector3 pos)
        {
            var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", pos, Quaternion.identity, true);
        }

        private void SetDummyAnimation(string name, bool isTrue)
        {
            foreach (var dummy in _dummyList)
            {
                dummy.SetAnimationParameter(name, isTrue);
            }

            // photonView.RPC("SetDummyAnimationRPC", RpcTarget.All, name, isTrue);
        }

        protected override void Dead()
        {
            base.Dead();

            _rasenganState = RasenganState.Ready;
            _isSkill_2KeyDown = false;
            _chargingGauge = 0;

            for (int i = 0; i < _dummyList.Count; i++)
            {
                Global.PoolingManager.LocalSpawn("Naruto_Smoke", _dummyList[i].transform.position,
                    _dummyList[i].transform.transform.rotation, true);
                PhotonNetwork.Destroy(_dummyList[i].gameObject);
            }

            _dummyList.Clear();
        }
    }
}