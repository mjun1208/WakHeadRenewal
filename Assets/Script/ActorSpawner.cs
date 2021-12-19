using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class ActorSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] List<GameObject> Actors;

        private Actor _currentActor = null;
        private int deadTime = 0;

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
            _currentActor.DeadTime =  _currentActor.IsLifeOn ? 0  : deadTime;

            float respawnTime = _currentActor.IsLifeOn ? 5f : 5f + (deadTime * 2f);
            
            yield return new WaitForSeconds(respawnTime);

            if (_currentActor.IsDead)
            {
                _currentActor.Respawn();
            }

            deadTime++;
        }
    }
}