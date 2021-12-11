using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class AnimalCrossing_FishBullet : ActorSub
    {
        [SerializeField] private List<GameObject> _fishs;
        private int _fishIndex;
        
        private Vector3 _originalScale;

        private readonly float attackDelay = 0.1f;
        private float _attackTimer = 0f;

        private void Awake()
        {
            _originalScale = this.transform.localScale;
        }
        
        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, int fishIndex, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = owner.transform.position + new Vector3(0, Random.Range(-0.5f, 0.5f));
            
            foreach (var fish in _fishs)
            {
                fish.SetActive(false);
            }

            _fishIndex = fishIndex;
            
            _fishs[fishIndex].SetActive(true);

            _attackRange = _fishs[fishIndex].GetComponent<AttackRange>();
            _attackRange.SetOwner(owner);
            
            float rotationScale = _originalScale.y * -dir.x;
            this.transform.localScale = new Vector3(_originalScale.x, rotationScale, _originalScale.z);
            
            _moveSpeed = Constant.ANIMALCROSSING_FISHBULLET_MOVE_SPEED;
            _lifeTime = Constant.ANIMALCROSSING_FISHBULLET_LIFETIME;

            StartCoroutine(Go());
        }

        private void Update()
        {
            if (_attackRange == null)
            {
                return;
            }

            _attackTimer += Time.deltaTime;
            
            if (_attackTimer > attackDelay)
            {
                _attackTimer = 0f;
                _attackRange.Attack(targetEntity => { OnDamage(targetEntity, 2 * _fishIndex); }, MyTeam);   
            }
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam, "AnimalCrossingSkill_2Effect", 0, false);
            }
        }

        protected override IEnumerator Go()
        {
            float goTime = 0;
            float moveAngle = 0;
            moveAngle = Random.Range(0, 360);

            while (goTime < _lifeTime)
            {
                // float x = Mathf.Cos(moveAngle * Mathf.Deg2Rad * 100f);
                float y = Mathf.Sin(moveAngle * Mathf.Deg2Rad * 100f);

                float angle = Mathf.Atan2(-y, -_dir.x) * Mathf.Rad2Deg;

                _dir = new Vector3(_dir.x, y, 0);

                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                goTime += Time.deltaTime;
                moveAngle += _moveSpeed * Time.deltaTime;

                _rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

                yield return null;
            }

            Destroy();
        }
    }
}