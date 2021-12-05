using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace WakHead
{
    public class Banana : Actor
    {
        [SerializeField] private GameObject _trampolinePivot;
     
        private List<Banana_Trampoline> _trampolineList = new List<Banana_Trampoline>();

        protected override void Update()
        {
            base.Update();
            
            for (int i = 0; i < _trampolineList.Count; i++)
            {
                if (_trampolineList[i].IsDead)
                {
                    var trampoline = _trampolineList[i];

                    photonView.RPC("SpawnDeadEffect", RpcTarget.All, trampoline.transform.position);

                    _trampolineList.Remove(trampoline);

                    PhotonNetwork.Destroy(trampoline.gameObject);
                }
            }
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            Vector2 dash = transform.position + GetAttackDir() * 10f * Time.deltaTime;
            _rigid.MovePosition(dash);
            
            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0, MyTeam,
                "NormalAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            SpawnTrampoline();
        }

        private void Active_Trampoline()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            
        }

        
        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("SpawnBall", RpcTarget.All);
        }
        
        public void SpawnTrampoline()
        {
            var newTrampoline =
                Global.PoolingManager.Spawn("Banana_Trampoline", _trampolinePivot.transform.position, Quaternion.identity);
            
            var newTrampolineScript = newTrampoline.GetComponent<Banana_Trampoline>();

            newTrampolineScript.SetInfo(this.photonView, this.gameObject, _trampolinePivot.transform.position, GetAttackDir(), MyTeam);
            
            _trampolineList.Add(newTrampolineScript);
        }
        
        [PunRPC]
        public void SpawnBall()
        {
            var newBullet =
                Global.PoolingManager.LocalSpawn("Normal_Bullet", this.transform.position, Quaternion.identity, true);

            newBullet.GetComponent<Normal_Bullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), MyTeam);
        }
        
        public void TrampolineColliderEnter(Collider2D collision)
        {
            if (collision.CompareTag("Trampoline"))
            {
                if (collision.GetComponent<Banana_Trampoline>().photonView.IsMine)
                {
                    Active_Trampoline();
                }
            }
        }
        
        [PunRPC]
        public void SpawnDeadEffect(Vector3 pos)
        {
            Global.PoolingManager.LocalSpawn("DeathEffect", pos, Quaternion.identity, true);
        }
    }
}