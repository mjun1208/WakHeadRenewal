using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Sans : Actor
    {
        private List<Sans_Gaster> _myGasterList = new List<Sans_Gaster>();

        protected override void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            base.Update();

            for (int i = 0; i < _myGasterList.Count; i++)
            {
                if (_myGasterList[i].IsDead)
                {
                    var gaster = _myGasterList[i];

                    photonView.RPC("SpawnDeadEffect", RpcTarget.All, gaster.transform.position);

                    _myGasterList.Remove(gaster);

                    PhotonNetwork.Destroy(gaster.gameObject);
                }
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("BoneAttack", RpcTarget.All);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            var newGaster =
                Global.PoolingManager.Spawn("Sans_Gaster", this.transform.position, this.transform.rotation);
            var newGasterScript = newGaster.GetComponent<Sans_Gaster>();
            newGasterScript.SetInfo(photonView.ViewID, this.gameObject, this.transform.position, GetAttackDir(), MyTeam);
            _myGasterList.Add(newGasterScript);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            foreach (var gaster in _myGasterList)
            {
                gaster.OnBlast();
            }
        }

        [PunRPC]
        public void BoneAttack()
        {
            var newbone =
                Global.PoolingManager.LocalSpawn("Sans_Bone", this.transform.position, Quaternion.identity, true);

            newbone.GetComponent<Sans_Bone>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }

        protected override void Dead()
        {
            base.Dead();

            for (int i = 0; i < _myGasterList.Count; i++)
            {
                Global.PoolingManager.LocalSpawn("DeathEffect", _myGasterList[i].transform.position,
                    _myGasterList[i].transform.transform.rotation, true);
                PhotonNetwork.Destroy(_myGasterList[i].gameObject);
            }

            _myGasterList.Clear();
        }


        [PunRPC]
        public void SpawnDeadEffect(Vector3 pos)
        {
            Global.PoolingManager.LocalSpawn("DeathEffect", pos, Quaternion.identity, true);
        }
    }
}