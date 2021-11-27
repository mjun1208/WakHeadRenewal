using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WakHead
{
    public class VR : Actor
    {
        [SerializeField] private GameObject _light;

        protected override void ForceStop()
        {
            base.ForceStop();

            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }

        protected override void Active_Attack()
        {
            _light.gameObject.SetActive(true);

            _light.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            _light.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.3f).SetEase(Ease.Flash)
                .OnComplete(() => { _light.gameObject.SetActive(false); });

            if (!photonView.IsMine)
            {
                return;
            }

            _attackRange.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, 5, MyTeam); });
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _skill_1Range.Attack(targetEntity => { targetEntity.Damaged(targetEntity.transform.position, 5, MyTeam); });
        }

        protected override void Attack()
        {
            base.Attack();

            if (_isAttackInput)
            {
                photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
            }
        }

        public override void OnSkill_1()
        {
            base.OnSkill_1();

            photonView.RPC("DisInvisibilityRPC", RpcTarget.All);
        }

        public override void OnSkill_2()
        {
            IsSkill_2 = true;
            IsDoingSkill = true;

            photonView.RPC("InvisibilityRPC", RpcTarget.All);
        }

        [PunRPC]
        public void InvisibilityRPC()
        {
            _animator.Rebind();

            float targetAlpha = photonView.IsMine ? 0.5f : 0f;

            _renderer.DOColor(new Color(1, 1, 1, targetAlpha), 0.8f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                IsSkill_2 = false;
                IsDoingSkill = false;
            });
        }

        [PunRPC]
        public void DisInvisibilityRPC()
        {
            _renderer.color = new Color(1, 1, 1, 1);
        }
    }
}