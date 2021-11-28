using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Vengenpro_Note : ActorSub
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private List<Sprite> _spriteList = new List<Sprite>();

        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            _renderer.sprite = _spriteList[Random.Range(0, _spriteList.Count)];

            _moveSpeed = Constant.VENGENPRO_NOTE_MOVE_SPEED;

            StartCoroutine(Go());
        }

        private void Update()
        {
            _attackRange.AttackEntity(targetEntity => { OnDamage(targetEntity, 5); }, MyTeam, true);
            _attackRange.AttackSummoned(targetSummoned =>
            {
                if (_ownerPhotonView.IsMine)
                {
                    targetSummoned.Damaged(targetSummoned.transform.position, MyTeam);
                }

                OnDamage(null, 5);
            }, MyTeam, true);
        }
    }
}