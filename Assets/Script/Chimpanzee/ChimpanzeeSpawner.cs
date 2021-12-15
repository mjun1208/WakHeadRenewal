using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class ChimpanzeeSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Team _team;

        private List<GameObject> _chimpanzeeList = new List<GameObject>();

        private void Start()
        {
            StartCoroutine(StartSpawn());
        }

        private IEnumerator StartSpawn()
        {
            yield return new WaitForSeconds(5f);
            
            while (true)
            {
                if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient &&
                    !string.IsNullOrWhiteSpace(Global.instance.MyActorName) &&
                    !string.IsNullOrWhiteSpace(Global.instance.EnemyActorName))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Spawn();
                    }
                }

                yield return new WaitForSeconds(20f);
            }
        }

        public void Spawn(bool isSuper = false)
        {
            string spawnName = "";

            switch (_team)
            {
                case Team.BLUE:
                {
                    spawnName = isSuper ? Constant.PANZEE_BLUE_TSHIRT : Constant.PANZEE_BLUE;

                    break;
                }
                case Team.RED:
                {
                    spawnName = isSuper ? Constant.PANZEE_RED_TSHIRT : Constant.PANZEE_RED;

                    break;
                }
            }

            var randomPos = (Vector3) Random.insideUnitCircle * 2.5f;

            var newPanzee =
                PhotonNetwork.Instantiate(spawnName, this.transform.position + randomPos, Quaternion.identity);
            newPanzee.GetComponent<Chimpanzee>().Init(_team);

            _chimpanzeeList.Add(newPanzee);
        }
    }
}