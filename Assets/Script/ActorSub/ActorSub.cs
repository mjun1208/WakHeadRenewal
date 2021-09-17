using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSub : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigid;
    [SerializeField] protected AttackRange _attackRange;

    protected GameObject _owner;
    protected Vector3 _dir;
    protected PhotonView _ownerPhotonView;

    public virtual void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        _ownerPhotonView = ownerPhotonView;

        _attackRange.SetOwner(owner);
        this.transform.position = owner.transform.position;

        _owner = owner;

        _dir = dir;
    }

    protected virtual void OnDamage(Entity entity)
    {
        StopAllCoroutines();

        if (!_ownerPhotonView.IsMine)
        {
            entity.Damaged(this.transform.position);
        }

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
