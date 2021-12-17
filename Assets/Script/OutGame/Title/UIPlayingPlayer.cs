using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayingPlayer : MonoBehaviour
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
        _lobbyPlayerCountText.text = $"게임 플레이 중 : {PhotonNetwork.CountOfPlayersInRooms} 명";
    }
}
