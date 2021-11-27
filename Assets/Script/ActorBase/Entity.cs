using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

namespace WakHead
{
    public abstract class Entity : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected Rigidbody2D _rigid;
        [SerializeField] protected SpriteRenderer _renderer;

        private ObscuredBool _knockBack = false;
        private ObscuredBool _grab = false;

        public Action StunAction;
        public Action CrownControlAction;
        public Action<Action, Vector3> DeadCameraAction;
        public Action DeadAction;

        public Team MyTeam { get; protected set; } = Team.None;

        private ObscuredInt _maxHP;
        private ObscuredInt _currentHP;

        public ObscuredBool IsStun
        {
            get { return _isStun; }
            protected set
            {
                if (value && _isStun != value)
                {
                    StunAction?.Invoke();
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
                if (value)
                {
                    if (DeadCameraAction != null)
                    {
                        DeadCameraAction?.Invoke(DeadAction, this.transform.position);
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
                stream.SendNext(_isHeart.GetDecrypted());
            }
            else
            {
                _maxHP = (int) stream.ReceiveNext();
                _currentHP = (int) stream.ReceiveNext();

                bool isDead = (bool) stream.ReceiveNext();
                bool isHeart = (bool) stream.ReceiveNext();

                if (IsDead != isDead)
                {
                    IsDead = isDead;
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

        public void SetTeam(Team team)
        {
            MyTeam = team;
        }

        public void OnCrownControl()
        {
            if (currentCrownControl != null)
            {
                StopCoroutine(currentCrownControl);
                _isHeart = false;

                photonView.RPC("ChangeColor", RpcTarget.All,
                    Color.white.r, Color.white.g, Color.white.b, Color.white.a);
            }

            currentCrownControl = null;
        }

        public void KnockBack(int damage, Vector3 dir, float power, float stunTime, string effectName = "HitEffect", bool effectFlip = false)
        {
            photonView.RPC("KnockBackRPC", RpcTarget.All, damage, dir, power, stunTime, effectName, effectFlip);
        }

        [PunRPC]
        public void KnockBackRPC(int damage, Vector3 dir, float power, float stunTime, string effectName = "HitEffect", bool effectFlip = false)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnKnockBack(damage, dir, power, stunTime, effectName, effectFlip);

            StartCoroutine(currentCrownControl);
        }

        public void Grab(int damage, Vector3 targetPostion, float grabSpeed, string effectName = "HitEffect", bool effectFlip = false)
        {
            photonView.RPC("GrabRPC", RpcTarget.All, damage, targetPostion, grabSpeed, effectName, effectFlip);
        }

        [PunRPC]
        public void GrabRPC(int damage, Vector3 targetPostion, float grabSpeed, string effectName = "HitEffect", bool effectFlip = false)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnGrab(damage, targetPostion, grabSpeed, effectName, effectFlip);

            StartCoroutine(currentCrownControl);
        }

        public void Stun(float stunTime)
        {
            photonView.RPC("StunRPC", RpcTarget.All, stunTime);
        }

        [PunRPC]
        public void StunRPC(float stunTime)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            CrownControlAction?.Invoke();

            currentCrownControl = OnStun(stunTime);

            StartCoroutine(currentCrownControl);
        }


        public void Heart()
        {
            photonView.RPC("HeartRPC", RpcTarget.All);
        }

        [PunRPC]
        public void HeartRPC()
        {
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

        private IEnumerator OnKnockBack(int damage, Vector3 dir, float power, float stunTime, string effectName = "HitEffect", bool effectFlip = false)
        {
            if (stunTime > 0)
            {
                IsStun = true;
            }

            Damaged(this.transform.position, damage, effectName, effectFlip);

            var targetPosition = this.transform.position + dir * power;

            float distance = float.MaxValue;

            while (distance > 0.3f)
            {
                distance = Vector3.Distance(this.transform.position, targetPosition);

                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 15f);

                yield return null;
            }

            yield return new WaitForSeconds(stunTime);

            IsStun = false;
        }

        private IEnumerator OnGrab(int damage, Vector3 targetPostion, float grabSpeed, string effectName = "HitEffect", bool effectFlip = false)
        {
            IsStun = true;

            float distance = float.MaxValue;

            while (distance > 0.1f)
            {
                distance = Vector3.Distance(this.transform.position, targetPostion);

                this.transform.position =
                    Vector3.Lerp(this.transform.position, targetPostion, Time.deltaTime * grabSpeed);

                yield return null;
            }

            Damaged(this.transform.position, damage, effectName, effectFlip);

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

        public void Damaged(Vector3 pos, int damage, string effectName = "HitEffect", bool effectFlip = false)
        {
            photonView.RPC("OnDamageRPC", RpcTarget.All, pos, damage, effectName, effectFlip);
        }

        [PunRPC]
        public void OnDamageRPC(Vector3 pos, int damage, string effectName = "HitEffect", bool effectFlip = false)
        {
            OnDamage(pos, damage, effectName, effectFlip);
        }

        public void OnDamage(Vector3 pos, int damage, string effectName = "HitEffect", bool effectFlip = false)
        {
            var randomPos = (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;

            Global.PoolingManager.LocalSpawn(effectName, this.transform.position + randomPos,
                Quaternion.Euler(new Vector3(0, effectFlip ? 0 : -180, 0)), true);

            if (photonView.IsMine)
            {
                HP -= damage;
            }
        }

        public void ResetHp()
        {
            HP = MaxHP;
        }
    }
}