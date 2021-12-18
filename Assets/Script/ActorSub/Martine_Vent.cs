using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace WakHead
{
    public class Martine_Vent : Summoned
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _arrow;

        private float _lifeTime = 0f;

        public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, Team team = Team.None)
        {
            MyTeam = team;
            
            MaxHP = 5;
            HP = MaxHP;
            
            _lifeTime = 0f;

            Select(false);
        }

        public void OnVent()
        {
            photonView.RPC("ActiveVent", RpcTarget.All);
        }

        private void Update()
        {
            _lifeTime += Time.deltaTime;
            
            if (_lifeTime > 5f)
            {
                IsDead = true;
            }
        }

        [PunRPC]
        private void ActiveVent()
        {
            _animator.Rebind();
            _animator.Play("Vent");
        }

        public void Select(bool isSelect)
        {
            _arrow.SetActive(isSelect);
        }
    }
}