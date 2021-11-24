using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace WakHead
{
    public class UIRoomList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _contentObject;
        [SerializeField] private GameObject _roonInfoObject;

        private List<UIRoomInfo> _roomLists = new List<UIRoomInfo>();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var room in roomList)
            {
                var removedRoom = _roomLists.Find(x => x.RoomName + x.RoomID == room.Name);

                if (removedRoom != null)
                {
                    _roomLists.Remove(removedRoom);
                    Destroy(removedRoom.gameObject);
                }

                if (room.RemovedFromList || room.MaxPlayers == room.PlayerCount)
                {
                    return;
                }
                else
                {
                    var newRoom = Instantiate(_roonInfoObject, _contentObject.transform);

                    var uiRoomInfo = newRoom.GetComponent<UIRoomInfo>();

                    string roomName = (string) room.CustomProperties["RN"];
                    string roomID = (string) room.CustomProperties["RI"];
                    uiRoomInfo.SetInfo(roomName, roomID);

                    _roomLists.Add(uiRoomInfo);
                }
            }
        }
    }
}