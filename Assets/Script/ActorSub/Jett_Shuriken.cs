using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jett_Shuriken : ActorSub
{
    [SerializeField] private TrailRenderer _trail;
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 pos, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        this.transform.position = pos;  

        _trail.Clear();

        _moveSpeed = Constant.JETT_SHURIKEN_MOVE_SPEED;

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

        StartCoroutine(Go());
    }

    private void Update()
    {
        _attackRange.AttackEntity(targetEntity =>
        {
            OnDamage(targetEntity);
        }, true);

        _attackRange.AttackSummoned(targetSummoned =>
        {
            targetSummoned.Damaged(targetSummoned.transform.position);
            OnDamage(null);
        }, true);
    }

    protected override void OnDamage(Entity entity)
    {
        StopAllCoroutines();

        if (_ownerPhotonView.IsMine)
        {
            entity?.KnockBack(_dir, 0.5f, 0f);
        }

        Destroy();
    }
}
