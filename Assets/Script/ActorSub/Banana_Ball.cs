using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana_Ball : Summoned, IPunObservable
    {
        [SerializeField] private Animator _animator;
        
        private bool _isRoll = false;
        private List<GameObject> _collidedObjectList = new List<GameObject>();
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_dir.x);
            }
            else
            {
                float rotationScale = _originalScale.x * (float) stream.ReceiveNext();
                this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            }
        }
        
        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            transform.position = pos;
            
            float rotationScale = _originalScale.x * dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            
            MaxHP = 5;
            HP = MaxHP;
        }

        public void HitBall(Vector3 dir)
        {
            photonView.RPC("StartRolling", RpcTarget.All, dir.x, dir.y);
            
            _rigid.velocity = Vector2.zero;
            Rolling();
        }

        public void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (_isRoll)
            {
                _attackRange.AttackEntity(targetEntity =>
                {
                    if (!_collidedObjectList.Contains(targetEntity.gameObject))
                    {
                        OnDamage(targetEntity, 15);
                        _collidedObjectList.Add(targetEntity.gameObject);
                    }
                }, MyTeam);

                
                _rigid.velocity = Vector2.Lerp(_rigid.velocity, Vector2.zero, 2f * Time.deltaTime);

                var velocityDistance = Vector2.Distance(_rigid.velocity, Vector2.zero);
                
                if (velocityDistance <= 0.5f)
                {
                    photonView.RPC("StopRolling", RpcTarget.All);
                }
            }
        }
        
        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam);
            }
        }


        [PunRPC]
        public void StartRolling(float dir_x, float dir_y)
        {
            _dir = new Vector3(dir_x > 0 ? 1 : -1, dir_y);
            
            float rotationScale = _originalScale.x * _dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
            
            _isRoll = true;
            _animator.SetBool("IsRoll", true);
            _collidedObjectList.Clear();
        }

        [PunRPC]
        public void StopRolling()
        {
            _isRoll = false;
            _animator.SetBool("IsRoll", false);
            _collidedObjectList.Clear();
        }
        
        public void Rolling()
        {
            _rigid.AddForce(_dir * 10f, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (photonView.IsMine)
            {
                TrampolineColliderEnter(other);
            }
        }

        public void TrampolineColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Trampoline"))
            {
                var dir = this.transform.position - collision.transform.position;
                dir.Normalize();
                HitBall(dir);
            }
        }
    }
}
