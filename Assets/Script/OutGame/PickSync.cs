using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PickManager.Instance.CurrentMyActorID);
            stream.SendNext(PickManager.Instance.IsMyReady);
        }
        else
        {
            PickManager.Instance.EnemyActorSelect((int)stream.ReceiveNext());
        
            bool isReady = (bool)stream.ReceiveNext();
            if (isReady != PickManager.Instance.IsEnemyReady)
            {
                PickManager.Instance.EnemyReady();
            }
            PickManager.Instance.IsEnemyReady = isReady;
        }
    }
}
