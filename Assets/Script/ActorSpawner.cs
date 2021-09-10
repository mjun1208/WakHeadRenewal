using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> Actors;

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Spawn(0);
        Spawn(1);
        Spawn(2);
        Spawn(3);
        Spawn(4);
    }


    void Spawn(int number)
    {
        if (Input.GetKeyDown(number.ToString()))
        {
            var o = Actors[number];
            var newobj = PhotonNetwork.Instantiate(o.name, Vector3.zero, Quaternion.identity);

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
