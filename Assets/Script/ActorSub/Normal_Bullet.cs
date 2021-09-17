using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Bullet : ActorSub
{
    public const float MoveSpeed = 15f;

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

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
            entity.KnockBack(_dir, 3f, 0f);
        }

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }

    private IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < 1f)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * MoveSpeed * Time.deltaTime);

            yield return null;
        }

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
