using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class Tower : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private Team _team;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _lineRendererPivot;
        
        [SerializeField] private Image _hpGauge;

        private Entity _targetEntity;
        private Actor _targetActor;

        private float _attackDelay = 0f;
        private readonly float attackDelay = 0.1f;

        public int HP
        {
            get { return _hp; }
            set
            {
                if (_hp != value)
                {
                    _hp = value;
                    _hpDownAction.Invoke();
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
            _maxHp = 5;
            _hp = 5;

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
            _hpDownAction += CheckWinTeam;
        }

        private void Update()
        {
            if (_attackDelay > 0f)
            {
                _attackDelay -= Time.deltaTime;
            }
            
            _targetEntity = GetAttackTarget();

            if (_targetEntity != null)
            {                
                _lineRenderer.SetPosition(0, _lineRendererPivot.transform.position);
                _lineRenderer.SetPosition(1, _targetEntity.transform.position);
                _lineRenderer.enabled = true;

                if (_attackDelay <= 0f)
                {
                    var isActor = _targetEntity is Actor;
                    
                    _targetEntity.OnDamage(_targetEntity.transform.position, isActor ? 5 : 1, _team);
                    _attackDelay = attackDelay;
                }
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

        public void MyActorDamaged()
        {
            if (GetEnemyActor() != null)
            {
                _targetEntity = _targetActor;
            }
        }
        
        private Actor GetEnemyActor()
        {
            int layerMask = (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 5f, Vector2.zero, 0f, layerMask);

            _targetActor = null;
            
            foreach (var hit in hits)
            {
                var hitActor = hit.transform.GetComponent<Actor>();
                
                if (hitActor != null && hitActor.MyTeam != _team) 
                {
                    _targetActor = hitActor;
                }
            }

            return _targetActor;
        }
        
        private Entity GetAttackTarget()
        {
            int layerMask = (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Player")) +
                            (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, 5f, Vector2.zero, 0f, layerMask);

            Entity targetEntity = null;
            Actor targetActor = null;
            
            foreach (var hit in hits)
            {
                var hitActor = hit.transform.GetComponent<Actor>();
                var hitChimpanzee = hit.transform.GetComponent<Chimpanzee>();

                if (hitChimpanzee != null && _targetEntity == hitChimpanzee)
                {
                    return hitChimpanzee;
                }
                
                if (hitActor != null && _targetEntity == hitActor)
                {
                    return hitActor;
                }

                if (hitChimpanzee != null && hitChimpanzee.MyTeam != _team && targetEntity == null) 
                {
                    targetEntity = hitChimpanzee;
                }
                
                if (hitActor != null && hitActor.MyTeam != _team) 
                {
                    targetActor = hitActor;
                }
            }

            if (targetEntity == null)
            {
                targetEntity = targetActor;
            }

            return targetEntity;
        }

        public void CheckWinTeam()
        {
            if (HP <= 0)
            {
                Global.instance.WinTeam(_team == Team.BLUE ? Team.RED : Team.BLUE);   
            }
        }
        
        public void SpawnHitEffect()
        {
            _hpGauge.fillAmount = (float)HP / (float)_maxHp;
            
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
