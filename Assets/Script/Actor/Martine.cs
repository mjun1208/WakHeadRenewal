using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Martine : Actor
    {
        private List<Martine_Vent> _myVentList = new List<Martine_Vent>();

        private Martine_Vent _currentVent = null;
        private Martine_Vent _ventingVent = null;

        private ObscuredBool _isOnVent = false;
        private ObscuredBool _isVenting = false;

        private List<Collider2D> _collidedVent = new List<Collider2D>();

        private IEnumerator _selectNextVent = null;

        protected override void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            base.Update();

            if (!_isVenting)
            {
                SelectVent();
            }

            for (int i = 0; i < _myVentList.Count; i++)
            {
                if (_myVentList[i].IsDead)
                {
                    var vent = _myVentList[i];

                    photonView.RPC("SpawnDeadEffect", RpcTarget.All, vent.transform.position);

                    _myVentList.Remove(vent);

                    var ventCollider = vent.GetComponent<Collider2D>();
                    if (_collidedVent.Contains(ventCollider))
                    {
                        _collidedVent.Remove(ventCollider);
                    }

                    PhotonNetwork.Destroy(vent.gameObject);
                }
            }
        }

        protected override void ForceStop()
        {
            base.ForceStop();
            if (_selectNextVent != null)
            {
                StopCoroutine(_selectNextVent);
            }

            _selectNextVent = null;
        }

        protected override void Flash()
        {
            if (!_isVenting)
            {
                base.Flash();
            }
        } 
        
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(5, GetAttackDir(), 0.5f, 0, MyTeam, 
                "Blood_2", GetAttackDir().x  * 0.3f, GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_1Range.Attack(targetEntity => { targetEntity.KnockBack(15, GetAttackDir(), 1f, 0, MyTeam, 
                "Blood_4", -GetAttackDir().x  * 0.3f, GetAttackDir().x < 0); }, MyTeam);
        }

        private void Active_Skill_1_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_1Range.Attack(targetEntity => { targetEntity.KnockBack(15, -GetAttackDir(), 1f, 0, MyTeam, 
                "Blood_4", GetAttackDir().x  * 0.3f, GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        public override void OnSkill_2()
        {
            if (_ventingVent != null)
            {
                return;
            }

            if (_currentVent == null)
            {
                var newVent =
                    Global.PoolingManager.Spawn("Martine_Vent", this.transform.position, this.transform.rotation);
                var newVentScript = newVent.GetComponent<Martine_Vent>();
                newVentScript.SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
                _myVentList.Add(newVentScript);
                
                Skill_2_Delay = Skill_2_CoolTime;
            }
            else
            {
                this.transform.position = _currentVent.transform.position + new Vector3(0, 0.2f, 0);
                base.OnSkill_2();

                _currentVent.OnVent();

                _collidedVent.Clear();

                _animator.SetBool("IsSkill_2", true);

                if (OnSkillCoroutine != null)
                {
                    StopCoroutine(OnSkillCoroutine);
                    OnSkillCoroutine = null;
                }

                IsDoingSkill = true;

                StartCoroutine(Venting());
            }
        }

        private int GetCurrentVentIndex()
        {
            int ventIndex = 0;
            int currentVentIndex = 0;

            foreach (var vent in _myVentList)
            {
                if (vent == _ventingVent)
                {
                    currentVentIndex = ventIndex;
                }

                ventIndex++;
            }

            return currentVentIndex;
        }

        [PunRPC]
        private void Hide(bool isTrue)
        {
            _renderer.enabled = !isTrue;
            _collider2D.enabled = !isTrue;
        }

        private IEnumerator Venting()
        {
            _isVenting = true;

            _ventingVent = _currentVent;
            int currentVentIndex = GetCurrentVentIndex();

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2"))
            {
                yield return null;
            }

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }

            photonView.RPC("Hide", RpcTarget.All, true);

            // 벤트 null 체크
            if (_ventingVent == null)
            {
                photonView.RPC("Hide", RpcTarget.All, false);

                if (_ventingVent != null)
                {
                    _currentVent = _ventingVent;
                    _currentVent.Select(true);

                    _ventingVent = null;
                }

                UpVent(this.transform.position);
            }

            // 벤트 선택
            _selectNextVent = SelectNextVent();
            StartCoroutine(_selectNextVent);
        }

        private IEnumerator SelectNextVent()
        {
            bool isInputUp = true;

            int currentIndex = GetCurrentVentIndex();

            Vector3 lastVentPos = _ventingVent.transform.position + new Vector3(0, 0.2f, 0);

            while (isInputUp)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _ventingVent.Select(false);

                    currentIndex = GetRightVent(GetCurrentVentIndex());

                    _ventingVent = _myVentList[currentIndex];
                    _ventingVent.Select(true);

                    lastVentPos = _ventingVent.transform.position + new Vector3(0, 0.2f, 0);

                    this.transform.position = lastVentPos;
                    _smoothSync.teleport();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _ventingVent.Select(false);

                    currentIndex = GetLeftVent(GetCurrentVentIndex());

                    _ventingVent = _myVentList[currentIndex];
                    _ventingVent.Select(true);

                    lastVentPos = _ventingVent.transform.position + new Vector3(0, 0.2f, 0);

                    this.transform.position = lastVentPos;
                    _smoothSync.teleport();
                }

                if (Input.GetKeyDown(KeyCode.C) || _ventingVent == null)
                {
                    isInputUp = false;
                }

                yield return null;
            }

            // 벤트 올라옴
            if (_ventingVent != null)
            {
                _ventingVent.OnVent();
            }

            yield return new WaitForSeconds(0.15f);

            photonView.RPC("Hide", RpcTarget.All, false);

            if (_ventingVent != null)
            {
                _currentVent = _ventingVent;
                _currentVent.Select(true);

                _ventingVent = null;
            }

            UpVent(lastVentPos);

            _selectNextVent = null;
        }

        private int GetRightVent(int currentIndex)
        {
            if (_myVentList.Count < 1)
            {
                return GetCurrentVentIndex();
            }

            Vector2 minDistance = new Vector2(float.MaxValue, float.MaxValue);

            int nextIndex = currentIndex;
            int ventIndex = 0;

            foreach (var vent in _myVentList)
            {
                Vector2 distance = vent.transform.position;

                if (_myVentList[currentIndex].transform.position.x < distance.x)
                {
                    if (minDistance.x > distance.x)
                    {
                        minDistance = distance;
                        nextIndex = ventIndex;
                    }
                }

                ventIndex++;
            }

            return nextIndex;
        }

        private int GetLeftVent(int currentIndex)
        {
            if (_myVentList.Count < 1)
            {
                return GetCurrentVentIndex();
            }

            Vector2 maxDistance = new Vector2(-float.MaxValue, -float.MaxValue);

            int nextIndex = currentIndex;
            int ventIndex = 0;

            foreach (var vent in _myVentList)
            {
                Vector2 distance = vent.transform.position;

                if (_myVentList[currentIndex].transform.position.x > distance.x)
                {
                    if (maxDistance.x < distance.x)
                    {
                        maxDistance = distance;
                        nextIndex = ventIndex;
                    }
                }

                ventIndex++;
            }

            return nextIndex;
        }

        private void UpVent(Vector3 lastVentPos)
        {
            // var nextVent = _myVentList[ventIndex];
            this.transform.position = lastVentPos;

            _smoothSync.teleport();

            _animator.SetBool("IsSkill_2", false);
            _animator.SetBool("IsSkill_2_Reverse", true);

            if (OnSkillCoroutine != null)
            {
                StopCoroutine(OnSkillCoroutine);
                OnSkillCoroutine = null;
            }

            IsDoingSkill = true;
            OnSkillCoroutine = OnSkill("Skill_2_Reverse");
            StartCoroutine(OnSkillCoroutine);

            _isVenting = false;
        }

        private void SelectVent()
        {
            if (_currentVent == null && _ventingVent == null && _collidedVent.Count > 0)
            {
                _currentVent = _collidedVent[0].GetComponent<Martine_Vent>();
                _currentVent.Select(true);
            }
        }

        public void VentColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Vent"))
            {
                if (collision.GetComponent<Martine_Vent>().photonView.IsMine)
                {
                    _collidedVent.Add(collision);
                }
            }
        }

        public void VentColliderExit(Collider2D collision)
        {
            if (collision.CompareTag("Vent"))
            {
                if (_currentVent != null)
                {
                    if (collision.gameObject == _currentVent.gameObject)
                    {
                        _currentVent.Select(false);
                        _currentVent = null;
                    }
                }

                _collidedVent.Remove(collision);
            }
        }

        [PunRPC]
        public void SpawnDeadEffect(Vector3 pos)
        {
            Global.PoolingManager.LocalSpawn("DeathEffect", pos, Quaternion.identity, true);
        }

        protected override void Dead()
        {
            base.Dead();

            if (_currentVent != null)
            {
                _currentVent.Select(false);
                _currentVent = null;
            }

            for (int i = 0; i < _myVentList.Count; i++)
            {
                Global.PoolingManager.LocalSpawn("DeathEffect", _myVentList[i].transform.position,
                    _myVentList[i].transform.transform.rotation, true);
                PhotonNetwork.Destroy(_myVentList[i].gameObject);
            }

            _myVentList.Clear();
            _collidedVent.Clear();
        }
    }
}