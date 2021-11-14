using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Title : MonoBehaviourPunCallbacks
{
    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    public byte MaxPlayers = 2;

    public int playerTTL = -1;

    private bool _isConnected = false;

    [SerializeField] private GameObject _waitImage;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected || !_isConnected)
        {
            return;
        }

        _waitImage.SetActive(true);

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;

        PhotonNetwork.CreateRoom("WAK", roomOptions, null);

        StartCoroutine(WaitPlayer());
    }

    public void JoinRoom()
    {
        _waitImage.SetActive(true);

        PhotonNetwork.JoinRandomRoom();

        StartCoroutine(WaitPlayer());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        PhotonNetwork.AutomaticallySyncScene = true;

        _isConnected = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one.");
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
    }

    private IEnumerator WaitPlayer()
    {
        while (!PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        yield return null;

        while (PhotonNetwork.CurrentRoom == null)
        {
            yield return null;
        }

        yield return null;

        while (PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayers)
        {
            yield return null;
        }

        yield return null;

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.LoadLevel("Pick");
    }
}
