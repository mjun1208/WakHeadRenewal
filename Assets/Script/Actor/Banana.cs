using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana : Actor
    {
        [SerializeField] private GameObject _trampolinePivot;
        [SerializeField] private GameObject _groundEffectPivot;
        private bool _isJump = false;
        private bool _isDown = false;

        private Vector3 _jumpPosition;
        
        private List<Banana_Trampoline> _trampolineList = new List<Banana_Trampoline>();

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            if (_isJump)
            {
                this.transform.Translate(Vector3.up * 30f * Time.fixedDeltaTime);

                JumpMove(_movedir);
            }
            
            if (_isDown)
            {
                if (this.transform.position.y > _jumpPosition.y)
                {
                    this.transform.Translate(Vector3.down * 30f * Time.fixedDeltaTime);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, _jumpPosition.y);
                }

                JumpMove(_movedir);
            }
        }

        protected override void Update()
        {
            base.Update();

            for (int i = 0; i < _trampolineList.Count; i++)
            {
                if (_trampolineList[i].IsDead)
                {
                    var trampoline = _trampolineList[i];

                    photonView.RPC("SpawnDeadEffect", RpcTarget.All, trampoline.transform.position);

                    _trampolineList.Remove(trampoline);

                    PhotonNetwork.Destroy(trampoline.gameObject);
                }
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            Vector2 dash = transform.position + GetAttackDir() * 10f * Time.deltaTime;
            _rigid.MovePosition(dash);
            
            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0, MyTeam,
                "NormalAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            SpawnTrampoline();
        }

        private void Active_Trampoline()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            IsSkill_1 = true;

            if (OnSkillCoroutine == null)
            {
                _animator.SetBool("IsSkill_1_Attack", true);

                IsDoingSkill = true;
                OnSkillCoroutine = OnSkill("Skill_1_Attack");
                StartCoroutine(OnSkillCoroutine);
            }
            else
            {
                IsSkill_1 = false;
            }
        }
        
        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("SpawnBall", RpcTarget.All);
        }
        
        public void SpawnTrampoline()
        {
            var newTrampoline =
                Global.PoolingManager.Spawn("Banana_Trampoline", _trampolinePivot.transform.position, Quaternion.identity);
            
            var newTrampolineScript = newTrampoline.GetComponent<Banana_Trampoline>();

            newTrampolineScript.SetInfo(this.photonView, this.gameObject, _trampolinePivot.transform.position, GetAttackDir(), MyTeam);
            
            _trampolineList.Add(newTrampolineScript);
        }

        public void ActiveJump()
        {
            _isJump = true;

            _jumpPosition = this.transform.position;
        }
        
        public void ActiveDown()
        {
            _isJump = false;
            _isDown = true;
        }

        public void StopDown()
        {
            _isJump = false;
            _isDown = false;

            this.transform.position = new Vector3(this.transform.position.x, _jumpPosition.y);


            SpawnGroundEffect();

            if (photonView.IsMine)
            {
                _skill_1Range.Attack(targetEntity =>
                {
                    var dir = targetEntity.transform.position - this.transform.position;
                    dir.Normalize();

                    targetEntity.KnockBack(10, dir, 1f, 0, MyTeam,
                        "NormalAttackEffect", dir.x * 0.1f, dir.x > 0);
                }, MyTeam);
            }
        }

        private void JumpMove(Vector3 moveDir)
        {
            _isMove = false;

            if (moveDir != Vector3.zero)
            {
                _isMove = true;
            }

            if (_isMove && moveDir.x != 0)
            {
                float rotation = 0;
                if (moveDir.x > 0)
                {
                    rotation = 1;
                }
                else
                {
                    rotation = -1;
                }

                float rotationScale = _originalScale.x * rotation;
                this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            }

            transform.Translate(moveDir * _moveSpeed * Time.fixedDeltaTime);
            _jumpPosition += moveDir * _moveSpeed * Time.fixedDeltaTime;
        }
        
        public void SpawnGroundEffect()
        {
            var groundEffect =
                Global.PoolingManager.LocalSpawn("GroundEffect", _groundEffectPivot.transform.position, Quaternion.identity, true);
        }

        [PunRPC]
        public void SpawnBall()
        {
            var newBullet =
                Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

            newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }
        
        public void TrampolineColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Trampoline"))
            {
                if (collision.GetComponent<Banana_Trampoline>().photonView.IsMine)
                {
                    Active_Trampoline();
                }
            }
        }
        
        [PunRPC]
        public void SpawnDeadEffect(Vector3 pos)
        {
            Global.PoolingManager.LocalSpawn("DeathEffect", pos, Quaternion.identity, true);
        }
    }
}