using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class BattleGround_Aim : ActorSub
    {
        [SerializeField] private GameObject _aimObject;
        [SerializeField] private SpriteRenderer _backGround;
        [SerializeField] private Animator _animator;

        public Vector3 AimPosition
        {
            get { return _aimObject.transform.position; }
            set { _aimObject.transform.position = value; }
        }

        public int ShootCount { get; private set; } = 5;

        private bool _isShooting = false;
        private IEnumerator _onShootingCoroutine = null;

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 position, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _onShootingCoroutine = null;
            _isShooting = false;
            _aimObject.transform.localPosition = Vector3.zero;

            ShootCount = 5;

            if (!_ownerPhotonView.IsMine)
            {
                _backGround.color = new Color(0, 0, 0, 0);
            }
            else
            {
                _backGround.color = new Color(1, 1, 1, 130f / 255f);
            }
            
            photonView.RPC("SetInfoRPC" , RpcTarget.All, position);
        }

        [PunRPC]
        public void SetInfoRPC(Vector3 position)
        {
            transform.position = position;
        }

        private void Update()
        {
            if (_ownerPhotonView == null || !_ownerPhotonView.IsMine || _isShooting)
            {
                return;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                AimPosition += Vector3.right * Constant.BATTLEGROUND_AIM_MOVE_SPEED * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                AimPosition += Vector3.left * Constant.BATTLEGROUND_AIM_MOVE_SPEED * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                AimPosition += Vector3.up * Constant.BATTLEGROUND_AIM_MOVE_SPEED * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                AimPosition += Vector3.down * Constant.BATTLEGROUND_AIM_MOVE_SPEED * Time.deltaTime;
            }
        }

        protected IEnumerator OnShooting()
        {
            _isShooting = true;

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Aim_Shoot"))
            {
                yield return null;
            }

            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }

            // end

            _isShooting = false;

            _onShootingCoroutine = null;

            if (ShootCount <= 0)
            {
                Destroy();
            }
        }

        public void Shoot(Vector3 shootPosition)
        {
            if (!this.gameObject.activeSelf && _isShooting)
            {
                return;
            }

            AimPosition = shootPosition;

            _onShootingCoroutine = OnShooting();
            StartCoroutine(_onShootingCoroutine);

            photonView.RPC("ShootRPC", RpcTarget.All);
        }

        [PunRPC]
        public void ShootRPC()
        {
            _animator.Play("Aim_Shoot");
        }

        public void ReduceShootCount()
        {
            ShootCount--;

            _attackRange.AttackEntity(targetEntity => { OnDamage(targetEntity, 15); }, MyTeam, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView != null && _ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 10);
            }, MyTeam, true);

            Global.PoolingManager.LocalSpawn("SnipeHitEffect", _aimObject.transform.position, Quaternion.identity,
                true);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView != null && _ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam,
                    "BattleGroundSkill_2Effect" , _dir.x * 0.01f);
            }
        }

        public void PlaySkill_2Sound()
        {
            Global.SoundManager.Play("BattleGround_Skill_2_Sound", this.transform.position);
        }
        
        public void PlaySkill_2_Start_Sound()
        {
            Global.SoundManager.Play("BattleGround_Skill_2_Start_Sound", this.transform.position);
        }
        
        public override void Destroy()
        {
            StopAllCoroutines();
            _onShootingCoroutine = null;

            DestoryAction?.Invoke(this);

            Global.PoolingManager.Despawn(this.gameObject);
        }
    }
}