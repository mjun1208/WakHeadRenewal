﻿using System;
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
        [SerializeField] private Text _gaugeText;
        
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
                
                switch (MyTeam)
                {
                    case Team.BLUE:
                    {
                        _renderer.color = Color.blue;
                        break;
                    }
                    case Team.RED:
                    {
                        _renderer.color = Color.red;
                        break;  
                    }
                }
                
                _gauge = (float) stream.ReceiveNext();

                _gaugeText.text = $"{Mathf.Round(_gauge)} / 100";
            }
        }

        private void Update()
        {
            foreach (var occupiedCollider in _collidedOccupiedColliderList)
            {
                UpdateGauge(occupiedCollider.MyTeam);
            }
        }

        private void UpdateGauge(Team team)
        {
            if (MyTeam == team)
            {
                _gauge += 10f * Time.deltaTime;
            }
            else
            {
                _gauge -= 20f * Time.deltaTime;

                if (_gauge <= 0f)
                {
                    _gauge = 0f;
                    MyTeam = team;

                    switch (team)
                    {
                        case Team.BLUE:
                        {
                            _renderer.color = Color.blue;
                            break;
                        }
                        case Team.RED:
                        {
                            _renderer.color = Color.red;
                            break;  
                        }
                    }
                }
            }
            
            _gaugeText.text = $"{Mathf.Round(_gauge)} / 100";
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