using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WakHead
{
    public class Tree : Actor
    {
        private Vector3 _realOriginalScale;
        
        protected override void Awake()
        {
            base.Awake();

            _realOriginalScale = _originalScale;
        }
        
        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.KnockBack(4, GetAttackDir(), 0.5f, 0, MyTeam,
                "TreeAttackEffect",GetAttackDir().x * 0.1f ,GetAttackDir().x > 0); }, MyTeam);
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _originalScale *= 1.2f;
            this.transform.localScale *= 1.2f;

            if ( HP + ((MaxHP - HP) * 0.2f) > MaxHP)
            {
                HP = MaxHP;
            }
            else
            {
                HP += (int)Mathf.Round(((MaxHP - HP) * 0.2f));
            }
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                var randomPos = (Vector3) Random.insideUnitCircle * 0.5f * this.transform.localScale.y;

                var leafScale = _originalScale.y / _realOriginalScale.y;

                photonView.RPC("ShakeLeaf", RpcTarget.All, randomPos.x, randomPos.y, leafScale);
            }
        }

        [PunRPC]
        public void ShakeLeaf(float randomPos_x, float randomPos_y, float leaftScale)
        {
            var newLeaf =
                Global.PoolingManager.LocalSpawn("Tree_Leaf", this.transform.position + new Vector3(randomPos_x, randomPos_y), Quaternion.identity, true);

            newLeaf.GetComponent<Tree_Leaf>().SetInfo(this.photonView, this.gameObject,
                this.transform.position + new Vector3(randomPos_x, randomPos_y), GetAttackDir(), leaftScale, MyTeam);
        }
        
        protected override void Dead()
        {
            base.Dead();

            _originalScale = _realOriginalScale;
            this.transform.localScale = _originalScale;
        }
    }
}
