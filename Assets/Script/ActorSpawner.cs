﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> Actors;

    private Actor _currentActor = null;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Global.instance.FadeOut();
        Spawn(Global.instance.MyActorID);
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    void Spawn(int number)
    {
        var o = Actors[number];
        var newobj = PhotonNetwork.Instantiate(o.name, Vector3.zero, Quaternion.identity);

        if (_currentActor != null)
        {
            PhotonNetwork.Destroy(_currentActor.gameObject);
        }

        _currentActor = newobj.GetComponent<Actor>();
        _currentActor.DeadAction += Respawn;

        Global.instance.SetMyActorName(o.name);

        photonView.RPC("SetEnemyActorName", RpcTarget.OthersBuffered, o.name);
        // PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    private void SetEnemyActorName(string name)
    {
        Global.instance.SetEnemyActorName(name);
    }

    private void Respawn()
    {
        StartCoroutine(RespawnTimer());
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(3f);

        _currentActor.Respawn();
    } 
}
