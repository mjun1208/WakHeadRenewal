using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace WakHead
{
    public class Title : MonoBehaviourPunCallbacks
    {
        /// <summary>Used as PhotonNetwork.GameVersion.</summary>
        public byte Version = 1;

        public byte MaxPlayers = 2;

        public int playerTTL = -1;

        private bool _isJoinLobby = false;
        private bool _isConnected = false;
        private bool _leaveRoom = false;

        [SerializeField] private GameObject _lobbyConnect;
        [SerializeField] private GameObject _nickName;
        [SerializeField] private GameObject _lobbyButton;
        [SerializeField] private GameObject _roomList;
        [SerializeField] private GameObject _waitImage;
        [SerializeField] private GameObject _controlWindow;

        private void Start()
        {
            _isJoinLobby = false;

            StartCoroutine(WaitConnectLobby());

            Time.timeScale = 1f;
            Global.instance.ResetInfo();

            if (PhotonNetwork.IsConnected)
            {
                _isConnected = true;
                _isJoinLobby = true;
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
            }
        }

        public void CreateRoom()
        {
            _waitImage.SetActive(true);
            _lobbyButton.SetActive(false);
            _roomList.SetActive(false);

            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = this.MaxPlayers,
                CustomRoomProperties = new Hashtable()
                {
                    {"RN", Global.instance.PlayerName.GetDecrypted()},
                    {"RI", PhotonNetwork.LocalPlayer.UserId}
                },
                CustomRoomPropertiesForLobby = new string[] {"RN", "RI"},
                IsVisible = true,
                IsOpen = true
            };
            if (playerTTL >= 0)
                roomOptions.PlayerTtl = playerTTL;

            PhotonNetwork.CreateRoom(Global.instance.PlayerName.GetDecrypted() + PhotonNetwork.LocalPlayer.UserId, roomOptions, null);

            StartCoroutine(WaitPlayer());
        }

        public void ShowRoomList(bool isShow)
        {
            _lobbyButton.SetActive(!isShow);
            _roomList.SetActive(isShow);
        }

        public void JoinRoom(string roomName, string roomID)
        {
            _waitImage.SetActive(true);
            _lobbyButton.SetActive(false);
            _roomList.SetActive(false);

            if (!PhotonNetwork.JoinRoom(roomName + roomID))
            {
                _waitImage.SetActive(false);
                ShowRoomList(false);
            }
            else
            {
                StartCoroutine(WaitPlayer());
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log(
                "OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" +
                PhotonNetwork.CloudRegion +
                "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");

            PhotonNetwork.AutomaticallySyncScene = true;

            _isConnected = true;

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            _isJoinLobby = true;
            
            Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion +
                      "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            GoLobby();
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" +
                      PhotonNetwork.CloudRegion + "], so we create one.");
        }

        // the following methods are implemented to give you some context. re-implement them as needed.
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom(p[[] called by PUN. Now this client is in a room in region [" +
                      PhotonNetwork.CloudRegion + "]. Game is now running.");
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                _leaveRoom = true;
            }
        }
        
        private IEnumerator WaitPlayer()
        {
            while (!PhotonNetwork.IsConnected)
            {
                yield return null;
            }

            yield return null;

            float timeOut = 5f;

            while (PhotonNetwork.CurrentRoom == null)
            {
                timeOut -= Time.deltaTime;

                if (timeOut <= 0f)
                {
                    Global.PoolingManager.SpawnNotifyText("방이 사라졌습니다..!!", -80f);

                    _waitImage.SetActive(false);
                    ShowRoomList(false);
                    yield break;
                }

                yield return null;
            }

            yield return null;

            while (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayers)
            {
                if (_leaveRoom)
                {
                    _leaveRoom = false;
                    PhotonNetwork.LeaveRoom();
                }

                yield return null;
            }

            yield return null;
            
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
            
                PhotonNetwork.LoadLevel("Pick");
            }
        }

        private IEnumerator WaitConnectLobby()
        {
            _lobbyConnect.SetActive(true);
            _nickName.SetActive(false);
            _lobbyButton.SetActive(false);
            _roomList.SetActive(false);

            yield return new WaitUntil(() => PhotonNetwork.IsConnected && _isConnected && Global.instance.IsLoaded && _isJoinLobby);
            
            _lobbyConnect.SetActive(false);

            GoLobby();

            yield return null;
        }

        public void GoLobby()
        {
            _waitImage.SetActive(false);

            if (string.IsNullOrWhiteSpace(Global.instance.PlayerName))
            {
                _nickName.SetActive(true);
                _lobbyButton.SetActive(false);
                _roomList.SetActive(false);
            }
            else
            {
                _lobbyButton.SetActive(true);
                _nickName.SetActive(false);
                _roomList.SetActive(false);
            }
        }

        public void SceneChangeActorIndex()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("ActorIndex");
        }
        
        public void SceneChangePlayGuide()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("PlayGuide");
        }
        
        public void HowToControl()
        {
            _controlWindow.SetActive(!_controlWindow.activeSelf);
        }
    }
}
