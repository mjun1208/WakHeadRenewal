﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Naruto_Rasengan : ActorSub
    {
        private Vector3 _originalScale;
        private int _size;

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, int size, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = pos;

            _moveSpeed = Constant.NARUTO_RASENGAN_MOVE_SPEED;

            _originalScale = new Vector3(2 + size, 2 + size, 2 + size);

            _size = size;

            float rotationScale = _originalScale.x * dir.x;
            this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

            StartCoroutine(Go());
            StartCoroutine(Spine());
        }

        private IEnumerator Spine()
        {
            while (true)
            {
                ActiveDamage();

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ActiveDamage()
        {
            _attackRange.Attack(targetEntity => { OnDamage(targetEntity, _size * 3); }, MyTeam);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam, "NarutoSkill_2Effect", 0, Random.Range(0, 2) == 0);
            }
        }
    }
}