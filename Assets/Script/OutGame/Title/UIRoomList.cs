using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UIRoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _contentObject;
    [SerializeField] private GameObject _roonInfoObject;

    private List<UIRoomInfo> _roomLists = new List<UIRoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                var removedRoom = _roomLists.Find(x => x.RoomName + x.RoomID == room.Name);

                if (removedRoom != null) 
                {
                    _roomLists.Remove(removedRoom);
                    Destroy(removedRoom.gameObject);
                }
            }
            else
            {
                var newRoom = Instantiate(_roonInfoObject, _contentObject.transform);

                var roomInfo = newRoom.GetComponent<UIRoomInfo>();
                
                string roomName = (string) room.CustomProperties["RN"];
                string roomID = (string) room.CustomProperties["RI"];
                roomInfo.SetInfo(roomName, roomID);
                
                _roomLists.Add(roomInfo);
            }
        }
    }
}
