using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : IOnEventCallback
{
    private Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, Queue<GameObject>> _localPool = new Dictionary<string, Queue<GameObject>>();

    public PoolingManager()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public GameObject Spawn(string prefabName, Vector3 position, Quaternion rotation, Action<GameObject> action = null)
    {
        if (!_pool.ContainsKey(prefabName))
        {
            _pool.Add(prefabName, new Queue<GameObject>());
        }

        var pool = _pool[prefabName];

        GameObject spawnObject = null;

        if (pool.Count > 0)
        {
            spawnObject = pool.Dequeue();

            spawnObject.transform.position = position;
            spawnObject.transform.rotation = rotation;
        }
        else
        {
            spawnObject = PhotonNetwork.Instantiate(prefabName, position, rotation);
            spawnObject.name = prefabName;
        }

        var targetID = spawnObject.GetPhotonView().ViewID;

        object[] content = new object[] { targetID , position, rotation};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(Constant.SPAWN_EVENT_ID, content, raiseEventOptions, sendOptions);

        spawnObject.SetActive(true);

        action?.Invoke(spawnObject);

        return spawnObject;
    }

    public void SpawnEvent(int targetID, Vector3 position, Quaternion rotation)
    {
        var targetObject = PhotonView.Find(targetID).gameObject;

        targetObject.transform.position = position;
        targetObject.transform.rotation = rotation;

        targetObject.SetActive(true);
    }

    public void Despawn(GameObject targetObject)
    {
        var targetID = targetObject.GetPhotonView().ViewID;

        object[] content = new object[] { targetID }; 
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(Constant.DESPAWN_EVENT_ID, content, raiseEventOptions, sendOptions);
    }

    public void DespawnEvent(int targetID)
    {
        var targetObject = PhotonView.Find(targetID).gameObject;
        var prefabName = targetObject.name;

        if (!_pool.ContainsKey(prefabName))
        {
            _pool.Add(prefabName, new Queue<GameObject>());
        }

        var pool = _pool[prefabName];

        pool.Enqueue(targetObject);
        targetObject.SetActive(false);
    }

    public GameObject LocalSpawn(string prefabName, Vector3 position, Quaternion rotation, bool isLocal = false)
    {
        if (!_localPool.ContainsKey(prefabName))
        {
            _localPool.Add(prefabName, new Queue<GameObject>());
        }

        var pool = _localPool[prefabName];

        GameObject spawnObject = null;

        if (pool.Count > 0)
        {
            spawnObject = pool.Dequeue();

            spawnObject.transform.position = position;
            spawnObject.transform.rotation = rotation;
        }
        else
        {
            spawnObject = GameObject.Instantiate(Global.ResourceManager.FindPrefab(prefabName), position, rotation);
            spawnObject.name = prefabName;
        }

        if (!isLocal)
        {
            object[] content = new object[] { prefabName, position, rotation};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(Constant.Local_SPAWN_EVENT_ID, content, raiseEventOptions, sendOptions);
        }

        spawnObject.SetActive(true);

        return spawnObject;
    }

    public void LocalSpawnEvent(string prefabName, Vector3 position, Quaternion rotation)
    {
        var targetObject = LocalSpawn(prefabName, position, rotation, true);

        targetObject.transform.position = position;
        targetObject.transform.rotation = rotation;

        targetObject.SetActive(true);
    }

    public void LocalDespawn(GameObject targetObject)
    {
        var prefabName = targetObject.name;

        if (!_localPool.ContainsKey(prefabName))
        {
            _localPool.Add(prefabName, new Queue<GameObject>());
        }

        var pool = _localPool[prefabName];

        pool.Enqueue(targetObject);
        targetObject.SetActive(false);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == Constant.SPAWN_EVENT_ID)
        {
            object[] data = (object[])photonEvent.CustomData;

            int targetID = (int)data[0];
            Vector3 targetPostion = (Vector3)data[1];
            Quaternion targetRotation = (Quaternion)data[2];

            SpawnEvent(targetID, targetPostion, targetRotation);
        }
        else if (eventCode == Constant.DESPAWN_EVENT_ID)
        {
            object[] data = (object[])photonEvent.CustomData;

            int targetID = (int)data[0];
            DespawnEvent(targetID);
        }
        else if (eventCode == Constant.Local_SPAWN_EVENT_ID)
        {
            object[] data = (object[])photonEvent.CustomData;

            string targetName = (string)data[0];
            Vector3 targetPostion = (Vector3)data[1];
            Quaternion targetRotation = (Quaternion)data[2];

            LocalSpawnEvent(targetName, targetPostion, targetRotation);
        }
    }
}
