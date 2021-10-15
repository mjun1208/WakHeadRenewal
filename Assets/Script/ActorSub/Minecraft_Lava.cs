using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Lava : ActorSub
{
    [SerializeField] private Dictionary<int, GameObject> _lavaDic = new Dictionary<int, GameObject>();

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;
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
