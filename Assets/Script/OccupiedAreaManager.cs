using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class OccupiedAreaManager : MonoBehaviourPunCallbacks
    {
        private List<OccupiedArea> _occupiedAreaList = new List<OccupiedArea>();
        
        private void Awake()
        {
            StartCoroutine(StartSpawn());
        }
        
        private IEnumerator StartSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(30f + Random.Range(0f, 15f));
                
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
            var randomPos = new Vector3(Random.Range(-5, 5f), Random.Range(-3f, 2f));

            var newOccupiedArea =
                PhotonNetwork.Instantiate("OccupiedArea", this.transform.position + randomPos, Quaternion.identity);

            _occupiedAreaList.Add(newOccupiedArea.GetComponent<OccupiedArea>());
            
            photonView.RPC("SpawnNotify", RpcTarget.All);
        }

        public void DespawnOccupiedAreaAll()
        {
            for (int i =0 ; i < _occupiedAreaList.Count; i++)
            {
                if (_occupiedAreaList[i] != null)
                {
                    PhotonNetwork.Destroy(_occupiedAreaList[i].gameObject);
                }
            }
        }

        [PunRPC]
        public void SpawnNotify()
        {
            Global.PoolingManager.SpawnNotifyText("점령 지역이 생성되었습니다.", Color.yellow);
        }
    }
}
