using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class UIOnlinePlayer : MonoBehaviour
    {
        [SerializeField] private Text _lobbyPlayerCountText;

        private void Start()
        {
            UpdatePlayerCount();
        }

        private void Update()
        {
            UpdatePlayerCount();
        }

        private void UpdatePlayerCount()
        {
            _lobbyPlayerCountText.text = $"로비 : {PhotonNetwork.CountOfPlayersOnMaster} 명";
        }
    }
}