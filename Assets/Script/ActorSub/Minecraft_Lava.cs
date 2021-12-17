using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Minecraft_Lava : ActorSub
    {
        [SerializeField] private List<GameObject> _lavaList = new List<GameObject>();

        public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir, Team team = Team.None)
        {
            base.SetInfo(ownerPhotonView, owner, dir, team);

            this.transform.position = pos;

            foreach (var lava in _lavaList)
            {
                lava.SetActive(false);
            }

            StartCoroutine(Fire());
            StartCoroutine(Diffuse());
        }

        public void ActiveDamage()
        {
            _attackRange.Attack(targetEntity => { OnDamage(targetEntity, 2); }, MyTeam);
        }

        protected override void OnDamage(Entity entity, int damage)
        {
            if (_ownerPhotonView.IsMine)
            {
                entity?.Damaged(this.transform.position, damage, MyTeam, "MinecraftSkill_2Effect" , _dir.x * 0.01f);
            }
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                ActiveDamage();
            }
        }

        private IEnumerator Diffuse()
        {
            foreach (var lava in _lavaList)
            {
                lava.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (var lava in _lavaList)
            {
                lava.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;

            Destroy();
        }

    }
}