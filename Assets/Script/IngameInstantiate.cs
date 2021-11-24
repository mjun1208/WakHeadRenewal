using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class IngameInstantiate : MonoBehaviour
    {
        private void Start()
        {
            Photon.Pun.PhotonNetwork.Instantiate("ActorSpawner", Vector3.zero, Quaternion.identity);
        }
    }
}