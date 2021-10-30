using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Slave : ActorSub
{
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _moveSpeed = Constant.MINECRAFT_SLAVE_MOVE_SPEED;

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

        StartCoroutine(Go());
        StartCoroutine(Spine());
    }

    private IEnumerator Spine()
    {
        while (true)
        {
            ActiveDamage();

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ActiveDamage()
    {
        _attackRange.Attack(targetEntity =>
        {
            OnDamage(targetEntity, 10);
        });
    }

    protected override void OnDamage(Entity entity, int damage)
    {
        var randomPos = (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f;
        var randomDir = randomPos.normalized;

        if (_ownerPhotonView.IsMine)
        {
            entity?.KnockBack(damage, randomDir, 1f, 0f);
        }
    }
}
