using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class OccupiedArea : MonoBehaviourPunCallbacks, IPunObservable
    {
        public Team MyTeam { get; private set; } = Team.None;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GameObject _gaugeObject;
        
        private List<OccupiedCollider> _collidedOccupiedColliderList = new List<OccupiedCollider>();
        private float _gauge = 0f;
        
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)MyTeam);
                stream.SendNext(_gauge);
            }
            else
            {
                MyTeam = (Team) stream.ReceiveNext();
                
                UpdateColor(MyTeam);
                
                _gauge = (float) stream.ReceiveNext();

                var gaugeScale = _gauge == 0f ? 0f : _gauge / 100;
            
                _gaugeObject.transform.localScale = new Vector3(gaugeScale, gaugeScale, gaugeScale);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _collidedOccupiedColliderList.Count; i++)
            {
                if (_collidedOccupiedColliderList[i].IsWork)
                {
                    UpdateGauge(_collidedOccupiedColliderList[i].MyTeam);
                }
            }
        }

        private void UpdateColor(Team team)
        {
            switch (team)
            {
                case Team.BLUE:
                {
                    _renderer.color = new Color(0,0,1, _renderer.color.a);
                    break;
                }
                case Team.RED:
                {
                    _renderer.color = new Color(1,0,0, _renderer.color.a);
                    break;  
                }
            }
        }
        
        private void UpdateGauge(Team team)
        {
            if (MyTeam == team)
            {
                _gauge += 15f * Time.deltaTime;
            }
            else
            {
                _gauge -= 15f * Time.deltaTime;

                if (_gauge <= 0f)
                {
                    _gauge = 0f;
                    MyTeam = team;

                    UpdateColor(team);
                }
            }
            
            if (_gauge >= 100)
            {
                SpawnSuperPanzee(team);
                PhotonNetwork.Destroy(this.gameObject);
            }

            var gaugeScale = _gauge == 0f ? 0f : _gauge / 100;
            
            _gaugeObject.transform.localScale = new Vector3(gaugeScale, gaugeScale, gaugeScale);
        }

        private void SpawnSuperPanzee(Team team)
        {
            if (Global.instance.MyTeam == team)
            {
                Global.SoundManager.Play("점령 성공", Vector3.zero);
            }
            
            switch (team)
            {
                case Team.BLUE:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Global.instance.BlueTower.GetComponent<ChimpanzeeSpawner>().Spawn(true);
                    }

                    break;
                }
                case Team.RED:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Global.instance.RedTower.GetComponent<ChimpanzeeSpawner>().Spawn(true);
                    }

                    break;  
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            if (!_collidedOccupiedColliderList.Contains(other.GetComponent<OccupiedCollider>()))
            {
                _collidedOccupiedColliderList.Add(other.GetComponent<OccupiedCollider>());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_collidedOccupiedColliderList.Contains(other.GetComponent<OccupiedCollider>()))
            {
                _collidedOccupiedColliderList.Remove(other.GetComponent<OccupiedCollider>());
            }
        }
    }
}