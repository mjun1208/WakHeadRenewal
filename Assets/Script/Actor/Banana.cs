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
        [SerializeField] private GameObject _ballPivot;
        [SerializeField] private GameObject _groundEffectPivot;
        [SerializeField] private GameObject _dropPoint;

        private bool _isJump = false;
        private bool _isDown = false;

        private Vector3 _jumpPosition;
        
        private List<Banana_Trampoline> _trampolineList = new List<Banana_Trampoline>();
        private List<Banana_Ball> _ballList = new List<Banana_Ball>();
        
        private List<Banana_Ball> _collidedBallList = new List<Banana_Ball>();

        private Banana_Trampoline _currentTrampoline;
        
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

            for (int i = 0; i < _ballList.Count; i++)
            {
                if (_ballList[i].IsDead)
                {
                    var ball = _ballList[i];

                    photonView.RPC("SpawnDeadEffect", RpcTarget.All, ball.transform.position);

                    _ballList.Remove(ball);
                    
                    if (_collidedBallList.Contains(ball))
                    {
                        _collidedBallList.Remove(ball);
                    }

                    PhotonNetwork.Destroy(ball.gameObject);
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

            foreach (var ball in _collidedBallList)
            {
                var dir = ball.transform.position - this.transform.position;
                dir.Normalize();
                ball.HitBall(GetAttackDir() + new Vector3(0, dir.y, 0));
            }
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            SpawnTrampoline();
        }

        private void Active_Trampoline(Banana_Trampoline trampoline)
        {
            if (IsSkill_1 || _isJump || _isDown)
            {
                return;
            }

            _animator.Rebind();
            
            _isJump = false;
            _isDown = false;
            _dropPoint.SetActive(false);

            _currentTrampoline = trampoline;
            
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

            SpawnBall();
        }
        
        public void SpawnTrampoline()
        {
            var newTrampoline =
                Global.PoolingManager.Spawn("Banana_Trampoline", _trampolinePivot.transform.position, Quaternion.identity);
            
            var newTrampolineScript = newTrampoline.GetComponent<Banana_Trampoline>();

            newTrampolineScript.SetInfo(this.photonView, this.gameObject, _trampolinePivot.transform.position, GetAttackDir(), MyTeam);
            
            _trampolineList.Add(newTrampolineScript);
        }
        
        public void SpawnBall()
        {
            var newBall =
                Global.PoolingManager.Spawn("Banana_Ball", _ballPivot.transform.position, Quaternion.identity);
            
            var newBallScript = newBall.GetComponent<Banana_Ball>();

            newBallScript.SetInfo(this.photonView, this.gameObject, _ballPivot.transform.position, GetAttackDir(), MyTeam);
            
            _ballList.Add(newBallScript);
        }

        public void ActiveJump()
        {
            _isJump = true;
            _ccImmunity = true;

            _jumpPosition = this.transform.position;

            if (photonView.IsMine)
            {
                if (_currentTrampoline != null)
                {
                    _currentTrampoline.Use();
                    _currentTrampoline = null;
                }

                _dropPoint.SetActive(true);
            }
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
            _ccImmunity = false;
            
            this.transform.position = new Vector3(this.transform.position.x, _jumpPosition.y);

            _dropPoint.SetActive(false);

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

            Vector3 movedPosition = moveDir * _moveSpeed * Time.fixedDeltaTime;
            
            if (Mathf.Abs(_jumpPosition.x + movedPosition.x) > 20)
            {
                movedPosition.x = 0f;
            }
            if (_jumpPosition.y + movedPosition.y > 0.5f)
            {
                movedPosition.y = 0f;
            }
            else if (_jumpPosition.y + movedPosition.y < -5.1f)
            {
                movedPosition.y = 0f;
            }
            
            transform.Translate(movedPosition);
            _jumpPosition += movedPosition;
            
            _dropPoint.transform.position = _jumpPosition;
        }
        
        public void SpawnGroundEffect()
        {
            var groundEffect =
                Global.PoolingManager.LocalSpawn("GroundEffect", _groundEffectPivot.transform.position, Quaternion.identity, true);
        }

        public void TrampolineColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Trampoline"))
            {
                var trampoline = collision.GetComponent<Banana_Trampoline>();
                
                if (trampoline.photonView.IsMine)
                {
                    Active_Trampoline(trampoline);
                }
            }
        }
        
        public void BallColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Ball"))
            {
                var ballScript = collision.GetComponent<Banana_Ball>();
                
                if (!_collidedBallList.Contains(ballScript) && ballScript.photonView.IsMine)
                {
                    _collidedBallList.Add(ballScript);
                }
            }
        }

        public void BallColliderExit(Collider2D collision)
        {
            if (collision.CompareTag("Ball"))
            {
                var ballScript = collision.GetComponent<Banana_Ball>();
                
                if (_collidedBallList.Contains(ballScript) && ballScript.photonView.IsMine)
                {
                    _collidedBallList.Remove(ballScript);
                }
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
            
            _isJump = false;
            _isDown = false;

            _dropPoint.SetActive(false);


            for (int i = 0; i < _trampolineList.Count; i++)
            {
                Global.PoolingManager.LocalSpawn("DeathEffect", _trampolineList[i].transform.position,
                    _trampolineList[i].transform.transform.rotation, true);
                PhotonNetwork.Destroy(_trampolineList[i].gameObject);
            }
            
            for (int i = 0; i < _ballList.Count; i++)
            {
                Global.PoolingManager.LocalSpawn("DeathEffect", _ballList[i].transform.position,
                    _ballList[i].transform.transform.rotation, true);
                PhotonNetwork.Destroy(_ballList[i].gameObject);
            }

            _trampolineList.Clear();
            _ballList.Clear();
            _collidedBallList.Clear();
        }
    }
}