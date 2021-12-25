using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Ahri_Orb : ActorSub
    {
        [SerializeField] private TrailRenderer _trail;

        public const float BackSpeed = 15f;

        private List<GameObject> _collidedObjectList = new List<GameObject>();

        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);
            
            StopAllCoroutines();
            
            _trail.Clear();

            _collidedObjectList.Clear();

            _moveSpeed = Constant.AHRI_ORB_MOVE_SPEED;

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntity =>
            {
                if (!_collidedObjectList.Contains(targetEntity.gameObject))
                {
                    OnDamage(targetEntity, 5);
                    _collidedObjectList.Add(targetEntity.gameObject);
                }
            } ,MyTeam);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (!_collidedObjectList.Contains(targetSummoned.gameObject))
                {
                    if (_ownerPhotonView.IsMine)
                    {
                        targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                        _collidedObjectList.Add(targetSummoned.gameObject);
                    }
                }
            } ,MyTeam ,this);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam, "AhriAttackEffect", -_dir.normalized.x * 0.3f ,_dir.x > 0);
            }
        }

        protected override IEnumerator Go()
        {
            float goTime = 0;

            while (goTime < 0.5f)
            {
                goTime += Time.deltaTime;
                _rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

                yield return null;
            }

            StartCoroutine(Back());
        }

        private IEnumerator Back()
        {
            _collidedObjectList.Clear();

            float ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

            while (ownerDistance > 0.2f)
            {
                _dir = _owner.transform.position - this.transform.position;
                _rigid.MovePosition(this.transform.position + _dir.normalized * BackSpeed * Time.deltaTime);

                ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

                yield return null;
            }

            _trail.Clear();

            Destroy();
        }
        
        public virtual void Destroy()
        {
            DestoryAction?.Invoke(this);
            this.gameObject.SetActive(false);
        }
    }
}