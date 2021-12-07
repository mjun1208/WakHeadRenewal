using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class OccupiedAreaManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            StartCoroutine(StartSpawn());
        }
        
        private IEnumerator StartSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(15f + Random.Range(0f, 5f));
                
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient /*&&
                    !string.IsNullOrWhiteSpace(Global.instance.MyActorName) &&
                    !string.IsNullOrWhiteSpace(Global.instance.EnemyActorName)*/)
                {
                    Spawn();
                }
            }
        }

        public void Spawn()
        {
            var randomPos = new Vector3(Random.Range(-5, 5f), Random.Range(-3, 3f));

            var newOccupiedArea =
                PhotonNetwork.Instantiate("OccupiedArea", this.transform.position + randomPos, Quaternion.identity);
            
            photonView.RPC("SpawnNotify", RpcTarget.All);
        }

        [PunRPC]
        public void SpawnNotify()
        {
            Global.PoolingManager.SpawnNotifyText("점령 지역이 생성되었습니다.", Color.yellow);
        }
    }
}
