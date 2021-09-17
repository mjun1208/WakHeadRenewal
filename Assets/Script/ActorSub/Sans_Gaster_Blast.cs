using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Gaster_Blast : ActorSub
{
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

    protected override void OnDamage(Entity entity)
    {
        if (!_ownerPhotonView.IsMine)
        {
            entity.Damaged(this.transform.position);
        }
    }
}
