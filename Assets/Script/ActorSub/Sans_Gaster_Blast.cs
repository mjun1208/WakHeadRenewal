using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Gaster_Blast : ActorSub
{
    private Vector3 _originalScale;
    private GameObject _gaster;

    public void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(PhotonView ownerPhotonView, GameObject gaster, GameObject owner, Vector3 pos, Vector3 dir)
    {
        _gaster = gaster;

        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);
    }

    private void Update()
    {
        if (_attackRange.CollidedObjectList.Count > 0)
        {
            foreach (var targetObject in _attackRange.CollidedObjectList)
            {
                OnDamage(targetObject.GetComponent<Entity>());
            }
        }
    }

    public override void Destroy()
    {
        Global.PoolingManager.LocalDespawn(_gaster);

        Global.PoolingManager.LocalSpawn("DeathEffect", this.transform.position, this.transform.rotation, true);

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }

    protected override void OnDamage(Entity entity)
    {
        if (_ownerPhotonView.IsMine)
        {
            entity?.Damaged(this.transform.position);
        }
    }
}
