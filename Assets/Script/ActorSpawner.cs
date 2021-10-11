using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> Actors;

    private GameObject _currentActor = null;

    private void Start()
    {
        Global.instance.FadeOut();
        Spawn(Global.instance.MyActorID);
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // for (int i = 0; i < Actors.Count; i++)
        // {
        //     Spawn(i);
        // }
    }


    void Spawn(int number)
    {
        var o = Actors[number];
        var newobj = PhotonNetwork.Instantiate(o.name, Vector3.zero, Quaternion.identity);

        if (_currentActor != null)
        {
            PhotonNetwork.Destroy(_currentActor);
        }

        _currentActor = newobj;

        Global.instance.SetMyActorName(o.name);
        photonView.RPC("SetEnemyActorName", RpcTarget.OthersBuffered, o.name);
        // PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    private void SetEnemyActorName(string name)
    {
        Global.instance.SetEnemyActorName(name);
    }
}
