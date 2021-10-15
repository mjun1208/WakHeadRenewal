using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Lava : ActorSub
{
    [SerializeField] private List<GameObject> _lavaList = new List<GameObject>();

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;

        foreach (var lava in _lavaList)
        {
            lava.SetActive(false);
        }

        StartCoroutine(Diffuse());
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

    private IEnumerator Diffuse()
    {
        foreach (var lava in _lavaList)
        {
            lava.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (var lava in _lavaList)
        {
            lava.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;

        Destroy();
    }

}
