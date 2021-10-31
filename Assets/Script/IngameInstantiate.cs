using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameInstantiate : MonoBehaviour
{
    private void Start()
    {
        Photon.Pun.PhotonNetwork.Instantiate("ActorSpawner", Vector3.zero, Quaternion.identity);
    }
}
