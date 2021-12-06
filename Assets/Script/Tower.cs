using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Tower : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private Team _team;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _lineRendererPivot;
        private Entity _targetEntity;

        public int HP
        {
            get { return _hp; }
            set
            {
                if (_hp != value)
                {
                    _hpDownAction.Invoke();
                    _hp = value;
                }
            }
        }

        private int _maxHp;
        private int _hp;

        private Action _hpDownAction;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(HP);
            }
            else
            {
                HP = (int) stream.ReceiveNext();
            }
        }

        private void Awake()
        {
            _maxHp = 20;
            _hp = 20;

            switch (_team)
            {
                case Team.BLUE:
                {
                    Global.instance.SetBlueTower(this);
                    break;
                }
                case Team.RED:
                {
                    Global.instance.SetRedTower(this);
                    break;
                }
            }

            _hpDownAction += SpawnHitEffect;
        }

        private void Update()
        {
            _targetEntity = GetAttackTarget();
            
            if (_targetEntity != null)
            {                
                _lineRenderer.SetPosition(0, _lineRendererPivot.transform.position);
                _lineRenderer.SetPosition(1, _targetEntity.transform.position);
                _lineRenderer.enabled = true;
                
                _targetEntity.OnDamage(_targetEntity.transform.position, 1, _team);
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }

        public void OnDamage()
        {
            HP -= 1;
        }

        private Entity GetAttackTarget()
        {
            int layerMask = (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 5f, Vector2.zero, 0f, layerMask);

            Entity targetEntitiy = null;
            
            foreach (var hit in hits)
            {
                var hitChimpanzee = hit.transform.GetComponent<Chimpanzee>();

                if (_targetEntity == hitChimpanzee)
                {
                    return hitChimpanzee;
                }

                if (hitChimpanzee.MyTeam != _team && targetEntitiy == null) 
                {
                    targetEntitiy = hitChimpanzee;
                }
            }

            return targetEntitiy;
        }

        public void SpawnHitEffect()
        {
            switch (_team)
            {
                case Team.BLUE:
                {
                    var hitEffect = Global.PoolingManager.LocalSpawn("BlueTowerHit", this.transform.position,
                        this.transform.rotation, true);
                    break;
                }
                case Team.RED:
                {
                    var hitEffect = Global.PoolingManager.LocalSpawn("RedTowerHit", this.transform.position,
                        this.transform.rotation, true);
                    break;
                }
            }
        }
    }
}
