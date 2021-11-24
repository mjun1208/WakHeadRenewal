using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class UIRoomInfo : MonoBehaviour
    {
        [SerializeField] private Title _title;
        [SerializeField] private Text _roomNameText;

        public string RoomName { get; private set; }
        public string RoomID { get; private set; }

        public void SetInfo(string roomName, string roomID)
        {
            RoomName = roomName;
            RoomID = roomID;

            _roomNameText.text = $"{RoomName} 님의 방";

            this.gameObject.SetActive(true);
        }

        public void JoinRoom()
        {
            _title.JoinRoom(RoomName, RoomID);
        }
    }
}