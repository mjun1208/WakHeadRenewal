using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jett_Shuriken : ActorSub
{
    [SerializeField] private TrailRenderer _trail;

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _trail.Clear();

        _moveSpeed = Constant.JETT_SHURIKEN_MOVE_SPEED;

        StartCoroutine(Go());
    }

    private void Update()
    {
        if (_attackRange.CollidedObjectList.Count > 0)
        {
            var targetObject = _attackRange.CollidedObjectList[0];
            OnDamage(targetObject.GetComponent<Entity>());
        }
    }

    protected override void OnDamage(Entity entity)
    {
        StopAllCoroutines();

        if (!_ownerPhotonView.IsMine)
        {
            entity.KnockBack(_dir, 0.5f, 0f);
        }

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
