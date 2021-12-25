using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Random = UnityEngine.Random;

namespace WakHead
{
    public abstract class Entity : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected Rigidbody2D _rigid;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] private GameObject _stunEffect;

        private ObscuredBool _knockBack = false;
        private ObscuredBool _grab = false;
        protected ObscuredBool _ccImmunity = false;

        public Action StunAction;
        public Action CrownControlAction;
        public Action<Action, Entity> DeadCameraAction;
        public Action DeadAction;
        public Action<bool> OnDamageAction;

        public Team MyTeam { get; protected set; } = Team.None;

        private ObscuredInt _maxHP;
        private ObscuredInt _currentHP;

        public ObscuredBool IsStun
        {
            get { return _isStun; }
            protected set
            {
                if (_isStun != value)
                {
                    if (value)
                    {
                        StunAction?.Invoke();
                    }
                    
                    EnableStunEffect(value);
                }

                _isStun = value;
            }
        }

        public ObscuredInt MaxHP
        {
            get { return _maxHP; }
            protected set { _maxHP = value; }
        }

        public ObscuredInt HP
        {
            get { return _currentHP; }
            protected set
            {
                _currentHP = value;

                if (_currentHP <= 0)
                {
                    IsDead = true;

                    _currentHP = 0;
                }
            }
        }

        public ObscuredBool IsDead
        {
            get { return _isDead; }
            protected set
            {
                if (value && IsDead != value)
                {
                    if (DeadCameraAction != null)
                    {
                        DeadCameraAction?.Invoke(DeadAction, this);
                    }
                    else
                    {
                        DeadAction?.Invoke();
                    }
                }

                _isDead = value;
            }
        }


        protected ObscuredBool _isStun = false;
        protected ObscuredBool _isDead = false;
        protected ObscuredBool _isHeart = false;

        private IEnumerator currentCrownControl = null;

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_maxHP.GetDecrypted());
                stream.SendNext(_currentHP.GetDecrypted());
                stream.SendNext(_isDead.GetDecrypted());
                stream.SendNext(_isStun.GetDecrypted());
                stream.SendNext(_isHeart.GetDecrypted());
                stream.SendNext(_ccImmunity.GetDecrypted());
            }
            else
            {
                _maxHP = (int) stream.ReceiveNext();
                _currentHP = (int) stream.ReceiveNext();

                bool isDead = (bool) stream.ReceiveNext();
                bool isStun = (bool) stream.ReceiveNext();
                bool isHeart = (bool) stream.ReceiveNext();
                _ccImmunity = (bool) stream.ReceiveNext();;
                
                if (IsDead != isDead)
                {
                    IsDead = isDead;
                }
                
                if (IsStun != isStun)
                {
                    IsStun = isStun;
                }

                if (_isHeart != isHeart)
                {
                    _isHeart = isHeart;
                }
            }
        }

        protected virtual void Awake()
        {
            CrownControlAction += OnCrownControl;
        }

        public virtual void SetTeam(Team team)
        {
            MyTeam = team;
        }

        public void OnCrownControl()
        {
            if (currentCrownControl != null)
            {
                StopCoroutine(currentCrownControl);
                _isHeart = false;
                IsStun = false;

                photonView.RPC("ChangeColor", RpcTarget.All,
                    Color.white.r, Color.white.g, Color.white.b, Color.white.a);
            }

            currentCrownControl = null;
        }

        public void KnockBack(int damage, Vector3 dir, float power, float stunTime, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            if (_ccImmunity)
            {
                Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);
                return;
            }
        
            photonView.RPC("KnockBackRPC", RpcTarget.All, damage, dir, power, stunTime, type, team, effectName, effectXOffset, effectFlip);
        }

        [PunRPC]
        public void KnockBackRPC(int damage, Vector3 dir, float power, float stunTime, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            if (!photonView.IsMine || (MyTeam != Team.None && MyTeam == team))
            {
                return;
            }

            if (_ccImmunity)
            {
                Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnKnockBack(damage, dir, power, stunTime, type, team, effectName, effectXOffset, effectFlip);

            StartCoroutine(currentCrownControl);
        }

        public void Grab(int damage, Vector3 targetPostion, float grabSpeed, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            if (_ccImmunity)
            {
                Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);
                return;
            }

            photonView.RPC("GrabRPC", RpcTarget.All, damage, targetPostion, grabSpeed, type, team, effectName, effectXOffset, effectFlip);
        }

        [PunRPC]
        public void GrabRPC(int damage, Vector3 targetPostion, float grabSpeed, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            if (!photonView.IsMine || (MyTeam != Team.None && MyTeam == team))
            {
                return;
            }
            
            if (_ccImmunity)
            {
                Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnGrab(damage, targetPostion, grabSpeed, type, team, effectName, effectXOffset, effectFlip);

            StartCoroutine(currentCrownControl);
        }

        public void Stun(float stunTime)
        {
            if (_ccImmunity)
            {
                return;
            }
        
            photonView.RPC("StunRPC", RpcTarget.All, stunTime);
        }
        
        public void ForceStun(float stunTime)
        {
            photonView.RPC("ForceStunRPC", RpcTarget.All, stunTime);
        }

        [PunRPC]
        public void StunRPC(float stunTime)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            if (_ccImmunity)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnStun(stunTime);

            StartCoroutine(currentCrownControl);
        }
        
        [PunRPC]
        public void ForceStunRPC(float stunTime)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnStun(stunTime);

            StartCoroutine(currentCrownControl);
        }

        public void EnableStunEffect(bool enable)
        {
            _stunEffect.SetActive(enable);
        }

        public void Heart(Team team)
        {
            if (_ccImmunity)
            {
                return;
            }
        
            photonView.RPC("HeartRPC", RpcTarget.All, team);
        }

        [PunRPC]
        public void HeartRPC(Team team)
        {
            if (MyTeam != Team.None && MyTeam == team)
            {
                return;
            }
            
            if (_ccImmunity)
            {
                return;
            }
            
            var heartParticle = Global.PoolingManager.LocalSpawn("HeartParticle",
                this.transform.position + new Vector3(0, 0, -1f), Quaternion.Euler(-90f, 0f, 0f), true);
            heartParticle.transform.parent = this.transform;
            heartParticle.transform.localPosition = Vector3.zero;
            heartParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            if (!photonView.IsMine)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnHeart();

            StartCoroutine(currentCrownControl);
        }

        private IEnumerator OnKnockBack(int damage, Vector3 dir, float power, float stunTime, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            if (stunTime > 0)
            {
                IsStun = true;
            }

            Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);

            var targetPosition = this.transform.position + dir * power;

            if (Mathf.Abs(targetPosition.x) > 20)
            {
                targetPosition.x = targetPosition.x > 0f ? 20f : -20f;
            }
            if (targetPosition.y > 0.4f)
            {
                targetPosition.y = 0.4f;
            }
            else if (targetPosition.y < -5.4f)
            {
                targetPosition.y = -5.4f;
            }
            
            float distance = float.MaxValue;

            float duration = distance / power;
            
            while (distance > 0.3f && duration > 0)
            {
                duration -= Time.deltaTime;
            
                distance = Vector3.Distance(this.transform.position, targetPosition);

                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 15f);

                yield return null;
            }

            yield return new WaitForSeconds(stunTime);

            IsStun = false;
        }

        private IEnumerator OnGrab(int damage, Vector3 targetPostion, float grabSpeed, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            IsStun = true;

            float distance = float.MaxValue;

            Damaged(this.transform.position, damage, type, team, effectName, effectXOffset, effectFlip);

            float duration = distance / grabSpeed;
            
            while (distance > 0.1f && duration > 0)
            {
                duration -= Time.deltaTime;
                
                distance = Vector3.Distance(this.transform.position, targetPostion);

                this.transform.position =
                    Vector3.Lerp(this.transform.position, targetPostion, Time.deltaTime * grabSpeed);

                yield return null;
            }

            yield return null;

            IsStun = false;
        }

        private IEnumerator OnStun(float stunTime)
        {
            IsStun = true;

            yield return new WaitForSeconds(stunTime);

            IsStun = false;
        }

        private IEnumerator OnHeart()
        {
            _isHeart = true;

            StunAction?.Invoke();

            photonView.RPC("ChangeColor", RpcTarget.All,
                Constant.HEART_COLOR.r, Constant.HEART_COLOR.g, Constant.HEART_COLOR.b, Constant.HEART_COLOR.a);

            yield return new WaitForSeconds(1.5f);

            _isHeart = false;

            photonView.RPC("ChangeColor", RpcTarget.All,
                Color.white.r, Color.white.g, Color.white.b, Color.white.a);
        }

        [PunRPC]
        public void ChangeColor(float r, float g, float b, float a)
        {
            _renderer.color = new Color(r, g, b, a);
        }

        public void Damaged(Vector3 pos, int damage, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        { 
            if (MyTeam != Team.None && MyTeam == team)
            {
                return;
            }

            photonView.RPC("OnDamageRPC", RpcTarget.All, pos, damage, type, team, effectName, effectXOffset, effectFlip);
        }

        [PunRPC]
        public void OnDamageRPC(Vector3 pos, int damage, AttackType type, Team team, string effectName = "HitEffect", float effectXOffset = 0f, bool effectFlip = false)
        {
            OnDamage(pos, damage, type, team, effectName, effectXOffset, effectFlip);
        }

        public void OnDamage(Vector3 pos, int damage, AttackType type, Team team, string effectName = "HitEffect",
            float effectXOffset = 0f, bool effectFlip = false)
        {
            if (MyTeam != Team.None && MyTeam == team)
            {
                return;
            }

            var randomPos = (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f;

            if (effectXOffset != 0f)
            {
                randomPos = new Vector3(effectXOffset, 0f);
            }

            var effect = Global.PoolingManager.LocalSpawn(effectName, this.transform.position + randomPos,
                Quaternion.Euler(new Vector3(0, effectFlip ? 0 : -180, 0)), true);

            if (effect != null && this.gameObject.activeSelf)
            {
                effect.transform.parent = this.transform;
            }

            if (type == AttackType.Actor)
            {
                Global.SoundManager.Play($"HitSound_{Random.Range(0, 3)}", this.transform.position);
            }

            OnDamageAction?.Invoke(effectName == "ChimpanzeeAttackEffect");
            
            if (photonView.IsMine)
            {
                HP -= damage;
            }
        }

        public void ResetHp()
        {
            HP = MaxHP;
        }
        
        protected Vector3 GetAttackDir()
        {
            float dir = transform.localScale.x > 0 ? 1 : -1;

            Vector3 attackDir = new Vector3(dir, 0, 0);

            return attackDir;
        }
    }
}