using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Bone : ActorSub
{
    [SerializeField] private Animator _animator;


    public const float X_OFFSET = 2f;

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = owner.transform.position + new Vector3(X_OFFSET * dir.x, 0, 0);

        _animator.Rebind();
        _animator.Play("Vent");

        _attackRange.CollidedObjectList.Clear();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            Global.PoolingManager.LocalDespawn(this.gameObject);
        }
    }

    private void OnDamage()
    {
        if (_ownerPhotonView.IsMine)
        {
            _attackRange.Attack(targetEntity =>
            {
                targetEntity.Damaged(this.transform.position, 10);
            });
        }
    }
}
