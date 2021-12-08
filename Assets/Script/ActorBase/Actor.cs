using Photon.Pun;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using Smooth;

namespace WakHead
{
    public abstract class Actor : Entity, IPunObservable
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected SmoothSyncPUN2 _smoothSync;
        [SerializeField] protected Collider2D _collider2D;

        [SerializeField] protected AttackRange _attackRange;
        [SerializeField] protected AttackRange _skill_1Range;
        [SerializeField] protected AttackRange _skill_2Range;

        [SerializeField] protected OccupiedCollider _occupiedCollider;

        protected ObscuredFloat _attackMoveSpeed = 4f;
        protected ObscuredFloat _moveSpeed = 8f;

        protected ObscuredBool _isMove = false;
        protected ObscuredBool _isMoveInput = false;
        protected ObscuredBool _isAttack = false;
        protected ObscuredBool _isAttackInput = false;

        protected Vector3 _movedir = Vector3.zero;

        protected ObscuredBool _isSkill_1Input = false;
        protected ObscuredBool _isSkill_2Input = false;

        public ObscuredFloat Skill_1_CoolTime { get; protected set; } = 0f;
        public ObscuredFloat Skill_2_CoolTime { get; protected set; } = 0f;

        public ObscuredFloat Skill_1_Delay { get; protected set; } = 0f;
        public ObscuredFloat Skill_2_Delay { get; protected set; } = 0f;

        public ObscuredBool IsSkill_1 { get; protected set; } = false;
        public ObscuredBool IsSkill_2 { get; protected set; } = false;

        protected Vector3 _originalScale = Vector3.zero;

        public ObscuredBool IsDoingSkill { get; protected set; } = false;

        public IEnumerator OnSkillCoroutine { get; protected set; } = null;

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(this.transform.localScale.x);
                stream.SendNext(this.transform.localScale.y);
                stream.SendNext(IsStun.GetDecrypted());
            }
            else
            {
                var scale_x = (float) stream.ReceiveNext();
                var scale_y = (float) stream.ReceiveNext();
                IsStun = (bool) stream.ReceiveNext();
                this.transform.localScale =
                    new Vector3(scale_x, scale_y, this.transform.localScale.z);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _originalScale = this.transform.localScale;
            StunAction += ForceStop;

            DeadCameraAction += CameraManager.instance.Dead; 
            DeadAction += Dead;

            MaxHP = 100;
            ResetHp();

            Skill_1_CoolTime = Global.GameDataManager.ActorGameData.ActorGameDataList[Global.instance.MyActorID]
                .Skill_1_CoolTime;
            Skill_2_CoolTime = Global.GameDataManager.ActorGameData.ActorGameDataList[Global.instance.MyActorID]
                .Skill_2_CoolTime;
        }

        protected virtual void Start()
        {
            SetActor();

            if (!photonView.IsMine)
            {
                this.gameObject.layer = 9; // Enemy;
                return;
            }

            this.gameObject.layer = 8; // Player;

            CameraManager.instance.SetTarget(this.transform);
        }
        
        public override void SetTeam(Team team)
        {
            base.SetTeam(team);
            
            _attackRange.SetTeam(team);
            _skill_1Range.SetTeam(team);
            _skill_2Range.SetTeam(team);
            _occupiedCollider.SetTeam(team);
        }

        public void SetActor()
        {
            if (photonView.IsMine)
            {
                Global.instance.SetMyActor(this);
            }
            else
            {
                Global.instance.SetEnemyActor(this);
            }
            
            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    this.transform.position = Global.instance.BlueTower.transform.position + new Vector3(-3f, 0, 0);
                    break;
                }
                case Team.RED:
                {
                    this.transform.position = Global.instance.RedTower.transform.position + new Vector3(3f, 0, 0);;
                    break;
                }
            }
            
            _smoothSync.teleport();
        }

        protected virtual void Update()
        {
            if (Skill_1_Delay > 0)
            {
                Skill_1_Delay -= Time.deltaTime;
            }
            else
            {
                Skill_1_Delay = 0;
            }
            
            if (Skill_2_Delay > 0)
            {
                Skill_2_Delay -= Time.deltaTime;
            }
            else
            {
                Skill_2_Delay = 0;
            }
            
            if (!photonView.IsMine || IsDead || IsStun)
            {
                return;
            }

            if (_isHeart)
            {
                if (Global.instance.EnemyActor != null)
                {
                    var enemyPos = Global.instance.EnemyActor.transform.position;
                    var dir = enemyPos - this.transform.position;
                    dir.Normalize();

                    _movedir = dir;

                    Move(_moveSpeed * 0.5f);
                }

                return;
            }

            CheckAttack();

            KeyInput();

            if (!IsDoingSkill)
            {
                Move();

                Attack();

                if (!_isAttackInput)
                {
                    if (_isSkill_1Input && Skill_1_Delay <= 0f)
                    {
                        OnSkill_1();
                        return;
                    }

                    if (_isSkill_2Input && Skill_2_Delay <= 0f)
                    {
                        OnSkill_2();
                        return;
                    }
                }
            }
        }

        protected virtual void CheckAttack()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                _isAttack = true;
            }
            else
            {
                _isAttack = false;
            }
        }
        
        protected virtual void ForceStop()
        {
            _animator.Rebind();
            SkillCancel();
        }

        protected virtual void KeyInput()
        {
            MoveInput();

            AttackInput();

            if (!_isAttackInput)
            {
                Skill_1Input();
                Skill_2Input();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Flash();
            }
        }

        protected virtual void Flash()
        {
            var flashDir = _movedir == Vector3.zero ? GetAttackDir() : _movedir;

            transform.Translate(flashDir * Constant.FLASH_OFFSET);
            _smoothSync.teleport();
        } 
        
        private void MoveInput()
        {
            _isMoveInput = false;

            _movedir = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _movedir = Vector3.left;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _movedir = Vector3.right;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _movedir += Vector3.up * 0.6f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _movedir += Vector3.down * 0.6f;
            }

            if (_movedir != Vector3.zero)
            {
                _isMoveInput = true;
            }
        }

        private void Move(float moveSpeed = 0f)
        {
            _isMove = false;

            if (_movedir != Vector3.zero)
            {
                _isMove = true;
            }

            if (_isMove && _movedir.x != 0)
            {
                float rotation = 0;
                if (_movedir.x > 0)
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

            if (moveSpeed == 0f)
            {
                moveSpeed = _isAttack ? _attackMoveSpeed : _moveSpeed;
            }

            Vector2 movedPosition = transform.position + _movedir * moveSpeed * Time.deltaTime;
            
            if (Mathf.Abs(movedPosition.x) > 20)
            {
                movedPosition.x = movedPosition.x > 0f ? 20f : -20f;
            }
            if (movedPosition.y > 0.4f)
            {
                movedPosition.y = 0.4f;
            }
            else if (movedPosition.y < -5.4f)
            {
                movedPosition.y = -5.4f;
            }

            _rigid.MovePosition(movedPosition);
            _animator.SetBool("IsWalk", _isMove);
        }

        protected virtual void AttackInput()
        {
            _isAttackInput = false;

            if (Input.GetKey(KeyCode.Z))
            {
                _isAttackInput = true;
            }
        }

        protected virtual void Attack()
        {
            _animator.SetBool("IsAttack", _isAttackInput);
        }


        protected virtual bool Skill_1Input()
        {
            _isSkill_1Input = false;

            if (Input.GetKeyDown(KeyCode.X))
            {
                _isSkill_1Input = true;
            }

            return _isSkill_1Input;
        }

        public virtual void OnSkill_1()
        {
            IsSkill_1 = true;

            if (OnSkillCoroutine == null)
            {
                IsDoingSkill = true;
                OnSkillCoroutine = OnSkill("Skill_1");
                StartCoroutine(OnSkillCoroutine);

                _animator.SetBool("IsSkill_1", true);

                Skill_1_Delay = Skill_1_CoolTime;
            }
            else
            {
                IsSkill_1 = false;
            }
        }

        protected virtual bool Skill_2Input()
        {
            _isSkill_2Input = false;

            if (Input.GetKeyDown(KeyCode.C))
            {
                _isSkill_2Input = true;
            }

            return _isSkill_2Input;
        }

        public virtual void OnSkill_2()
        {
            IsSkill_2 = true;

            if (OnSkillCoroutine == null)
            {
                IsDoingSkill = true;
                OnSkillCoroutine = OnSkill("Skill_2");
                StartCoroutine(OnSkillCoroutine);

                _animator.SetBool("IsSkill_2", true);
                
                Skill_2_Delay = Skill_2_CoolTime;
            }
            else
            {
                IsSkill_2 = false;
            }
        }

        protected IEnumerator OnSkill(string name)
        {
            IsDoingSkill = true;

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                yield return null;
            }

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
            {
                yield return null;
            }

            // end

            IsDoingSkill = false;
            IsSkill_1 = false;
            IsSkill_2 = false;

            _animator.SetBool("Is" + name, false);

            OnSkillCoroutine = null;
        }

        protected void SkillCancel()
        {
            if (OnSkillCoroutine != null)
            {
                StopCoroutine(OnSkillCoroutine);
                OnSkillCoroutine = null;
            }

            IsDoingSkill = false;
            IsSkill_1 = false;
            IsSkill_2 = false;

            _animator.SetBool("IsSkill_1", false);
            _animator.SetBool("IsSkill_2", false);
        }

        protected virtual void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        protected virtual void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        protected virtual void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        protected virtual void Dead()
        {
            _isStun = false;
            _isHeart = false;
            _ccImmunity = false;

            var deathEffect = Global.PoolingManager.LocalSpawn("DeathEffect", this.transform.position,
                this.transform.rotation, true);
            _renderer.enabled = false;
            _collider2D.enabled = false;

            ForceStop();
        }

        public void Respawn()
        {
            IsDead = false;
            ResetHp();

            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    transform.position = Global.instance.BlueTower.transform.position + new Vector3(-3f, 0, 0);
                    break;
                }
                case Team.RED:
                {
                    transform.position = Global.instance.RedTower.transform.position + new Vector3(3f, 0, 0);;
                    break;
                }
            }
            
            _smoothSync.teleport();

            photonView.RPC("RespawnRPC", RpcTarget.All);
        }

        [PunRPC]
        public void RespawnRPC()
        {
            _animator.Rebind();
            _renderer.enabled = true;
            _collider2D.enabled = true;
        }
    }
}