using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WakHead
{
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
                int enemyActorID = (int) stream.ReceiveNext();

                if (enemyActorID != -1)
                {
                    PickManager.Instance.EnemyActorSelect(enemyActorID);
                }

                bool isReady = (bool) stream.ReceiveNext();
                if (isReady != PickManager.Instance.IsEnemyReady)
                {
                    PickManager.Instance.EnemyReady();
                }

                PickManager.Instance.IsEnemyReady = isReady;
            }
        }

        public void StartGame()
        {
            photonView.RPC("StartGameRPC", RpcTarget.Others);
        }

        [PunRPC]
        public void StartGameRPC()
        {
            PickManager.Instance.StartGameClient();
        }
    }
}