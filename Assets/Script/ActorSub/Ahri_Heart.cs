using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri_Heart : ActorSub
{
    public const float MoveSpeed = 8f;

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
