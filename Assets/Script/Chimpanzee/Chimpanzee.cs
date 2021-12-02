using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Chimpanzee : Entity, IPunObservable
    {
        private enum ChimpanzeeState
        {
            Move,
            Attack
        }

        public bool IsAttack { get; private set; } = false;
        public bool IsTowerInAttackRange { get; private set; } = false;

        [SerializeField] protected Animator _animator;
        [SerializeField] protected AttackRange _attackRange;

        private const float moveSpeed = 1.5f;

        private ChimpanzeeState _state = ChimpanzeeState.Move;
        private Entity _targetEntity;
        private Tower _targetTower;

        private Vector3 _originalScale = Vector3.zero;

        private float _attackDelay = 0;

        private readonly Vector3 TowerOffset = new Vector3(0, -1, 0);
        
        private Vector3 _currentPosition;

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(this.transform.localScale.x);
            }
            else
            {
                var scale_x = (float) stream.ReceiveNext();
                this.transform.localScale =
                    new Vector3(scale_x, this.transform.localScale.y, this.transform.localScale.z);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _originalScale = this.transform.localScale;

            CrownControlAction += () =>
            {
                _state = ChimpanzeeState.Move;

                _animator.SetBool("isAttack", false);

                _rigid.isKinematic = false;
                
                _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

                _attackDelay = 0.5f;
                
                IsAttack = false;
            };

            MaxHP = 50;
            ResetHp();

            DeadAction += Dead;
        }

        [PunRPC]
        private void SetTeamRPC(Team team)
        {
            SetTeam(team);
            _attackRange.SetTeam(team);
        }
        
        public void Init(Team team)
        {
            photonView.RPC("SetTeamRPC", RpcTarget.All, team);
            
            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    _targetTower = Global.instance.RedTower;

                    break;
                }
                case Team.RED:
                {
                    _targetTower = Global.instance.BlueTower;

                    break;
                }
            }
        }

        public void SetTarget(Entity entity)
        {
            _targetEntity = entity;
        }

        private void Update()
        {
            if (!photonView.IsMine || IsDead || IsStun)
            {
                return;
            }

            if (_isHeart)
            {
                HeartMove();
                return;
            }

            if (_attackDelay > 0)
            {
                _attackDelay -= Time.deltaTime;
            }
            else
            {
                _attackDelay = 0f;
            }


            var towerDistance =
                Vector3.Distance(_targetTower.transform.position + TowerOffset, this.transform.position);
            IsTowerInAttackRange = towerDistance <= 0.5f;

            switch (_state)
            {
                case ChimpanzeeState.Move:
                {
                    if (_attackDelay <= 0)
                    {
                        Move();

                        if (_attackRange.CollidedObjectList.Count > 0 || IsTowerInAttackRange || GetAttackTarget())
                        {
                            _state = ChimpanzeeState.Attack;

                            _animator.SetBool("isAttack", true);

                            _rigid.isKinematic = true;
                            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                            IsAttack = true;
                        }
                    }

                    break;
                }
                case ChimpanzeeState.Attack:
                {
                    _currentPosition = this.transform.position;
                    
                    if (_attackDelay <= 0)
                    {
                        Attack();
                    }

                    if (_attackRange.CollidedObjectList.Count == 0 && !IsTowerInAttackRange)
                    {
                        _state = ChimpanzeeState.Move;

                        _animator.SetBool("isAttack", false);

                        _rigid.isKinematic = false;
                        _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        IsAttack = false;
                    }

                    break;
                }
            }
        }

        private bool GetAttackTarget()
        {
            float maxDistance = 0f;
            int layerMask = (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.BoxCastAll(this.transform.position + GetAttackDir() * 0.1f * 1.5f, new Vector2(0.2f * 1.5f,0.29f * 1.5f), 0f, GetAttackDir(), maxDistance, layerMask);

            foreach (var hit in hits)
            {
                var hitChimpanzee = hit.transform.GetComponent<Chimpanzee>();

                if (hitChimpanzee.MyTeam != MyTeam)
                {
                    return true;
                }
            }

            return false;
        }
        
        private void Move()
        {
            if (_targetEntity == null)
            {
                Vector3 dir = _targetTower.transform.position + TowerOffset - this.transform.position;
                dir = dir.normalized;

                if (dir.x != 0)
                {
                    float rotation = 0;
                    if (dir.x > 0)
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

                float dirY = GetYDir();
                dir.y = dirY == 0 ? dir.y : dirY;
                
                _rigid.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
            }
        }

        private float GetYDir()
        {
            float maxDistance = 1f;
            int layerMask = (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.BoxCastAll(this.transform.position + GetAttackDir() * 0.15f, new Vector2(1f,1f), 0f, GetAttackDir(), maxDistance, layerMask);

            foreach (var hit in hits)
            {
                var hitChimpanzee = hit.transform.GetComponent<Chimpanzee>();

                if (hitChimpanzee.IsAttack && !hitChimpanzee.IsTowerInAttackRange && hitChimpanzee.MyTeam == MyTeam)
                {
                    float dirY = hit.transform.position.y > this.transform.position.y ? -1f : 1f;

                    return dirY;
                }
            }

            return 0f;
        }

        private void HeartMove()
        {
            Actor enemy = null;

            if (MyTeam == Global.instance.MyTeam)
            {
                enemy = Global.instance.EnemyActor;
            }
            else
            {
                enemy = Global.instance.MyActor;
            }

            if (enemy != null)
            {
                var enemyPos = enemy.transform.position;
                var dir = enemyPos - this.transform.position;
                dir.Normalize();

                if (dir.x != 0)
                {
                    float rotation = 0;
                    if (dir.x > 0)
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

                _rigid.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
            }
        }

        private void Attack()
        {
            if (IsTowerInAttackRange)
            {
                _targetTower.OnDamage();
            }
            else
            {
                _attackRange.Attack(targetEntity =>
                {
                    targetEntity.Damaged(targetEntity.transform.position, 1, MyTeam, "ChimpanzeeAttackEffect");
                    //targetEntity.KnockBack(GetAttackDir(), 0.5f, 0);
                }, MyTeam);
            }

            _attackDelay = 0.2f;
        }

        private void Dead()
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            var deathEffect = Global.PoolingManager.LocalSpawn("DeathEffect", this.transform.position,
                this.transform.rotation, true);
        }
    }
}