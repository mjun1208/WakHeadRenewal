using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri_Heart : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private AttackRange _attackRange;
    private Vector3 _dir;
    private PhotonView _ownerPhotonView;

    public const float MoveSpeed = 8f;

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        _ownerPhotonView = ownerPhotonView;

        _attackRange.SetOwner(owner);
        this.transform.position = owner.transform.position;

        _dir = dir;

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

    private void OnDamage(Entity entity)
    {
        StopAllCoroutines();

        if (!_ownerPhotonView.IsMine)
        {
            entity.Damaged();
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
