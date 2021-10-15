using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Naruto_Rasengan : ActorSub
{    
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;

        _moveSpeed = Constant.NARUTO_RASENGAN_MOVE_SPEED;

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

        StartCoroutine(Go());
    }

    public void ActiveDamage()
    {
        if (_attackRange.CollidedObjectList.Count > 0)
        {
            foreach (var targetObject in _attackRange.CollidedObjectList)
            {
                OnDamage(targetObject.GetComponent<Entity>());
            }
        }
    }

    protected override void OnDamage(Entity entity)
    {
        if (_ownerPhotonView.IsMine)
        {
            entity.Damaged(this.transform.position);
        }
    }
}
