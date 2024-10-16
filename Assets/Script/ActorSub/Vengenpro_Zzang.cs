﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Vengenpro_Zzang : ActorSub
    {
        private List<GameObject> _collidedObjectList = new List<GameObject>();
        private Vector3 _originScale;

        private void Awake()
        {
            _originScale = this.transform.localScale;
        }

        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.localScale = _originScale;

            _moveSpeed = Constant.VENGENPRO_ZZANG_MOVE_SPEED;

            _collidedObjectList.Clear();

            StartCoroutine(Go());
        }

        private void Update()
        {
            this.transform.localScale += new Vector3(1f, 1f, 1f) * _moveSpeed * Time.deltaTime;

            _attackRange.AttackEntity(targetEntity =>
            {
                if (!_collidedObjectList.Contains(targetEntity.gameObject))
                {
                    OnDamage(targetEntity, 10);
                    _collidedObjectList.Add(targetEntity.gameObject);
                }
            }, MyTeam);
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
            }, MyTeam, this);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, AttackType.Actor, MyTeam, "VengenproSkill_1Effect", _dir.x * 0.01f);
            }
        }
    }
}