﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WakHead
{
    public class PickSync : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static Dictionary<string, PickSync> Instance = new Dictionary<string, PickSync>();

        private void Awake()
        {
            if (!photonView.IsMine)
            {
                Global.instance.SetEnemyName(photonView.Owner.NickName);
            }

            string playerName = photonView.IsMine ? Global.instance.PlayerName : Global.instance.EnemyName;

            if (Instance.ContainsKey(playerName))
            {
                var instance = Instance[playerName];
                Instance.Remove(playerName);
                
                if (instance != null)
                {
                    PhotonNetwork.Destroy(instance.gameObject);
                }
            }

            Instance.Add(playerName, this);
            PickManager.Instance.MyPickSync = this;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PickManager.Instance.CurrentMyActorID);
                stream.SendNext(PickManager.Instance.IsMyReady);
            }
            else
            {
                int enemyActorID = (int) stream.ReceiveNext();

                if (enemyActorID != -1)
                {
                    Global.instance.SetEnemyActorID(enemyActorID);
                }

                bool isReady = (bool) stream.ReceiveNext();
                if (isReady != PickManager.Instance.IsEnemyReady)
                {
                    PickManager.Instance.EnemyReady();
                }

                PickManager.Instance.IsEnemyReady = isReady;
            }
        }

        public void StartGame()
        {
            Instance.Clear();
            photonView.RPC("StartGameRPC", RpcTarget.Others);
        }

        [PunRPC]
        public void StartGameRPC()
        {
            Instance.Clear();
            PickManager.Instance.StartGameClient();
        }
    }
}