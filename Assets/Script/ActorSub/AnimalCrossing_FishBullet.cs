using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCrossing_FishBullet : ActorSub
{
    [SerializeField] private List<GameObject> _fishs;

    private Vector3 _originalPos;

    public const float MoveSpeed = 5f;
    public const float Offset = 1f;

    public void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir, int fishIndex)
    {
        _ownerPhotonView = ownerPhotonView;
        _owner = owner;

        foreach (var fish in _fishs)
        {
            fish.SetActive(false);
        }

        _fishs[fishIndex].SetActive(true);

        _attackRange = _fishs[fishIndex].GetComponent<AttackRange>();
        _attackRange.SetOwner(owner);

        this.transform.position = owner.transform.position;
        _originalPos = owner.transform.position;

        StartCoroutine(Go());
    }

    private void Update()
    {
        if (_attackRange == null)
        {
            return;
        }

        if (_attackRange.CollidedObjectList.Count > 0)
        {
            var targetObject = _attackRange.CollidedObjectList[0];
            OnDamage(targetObject.GetComponent<Entity>());
        }
    }

    protected override void OnDamage(Entity entity)
    {
        // StopAllCoroutines();

        if (!_ownerPhotonView.IsMine)
        {
            entity.Damaged(this.transform.position);
        }

        // Global.PoolingManager.LocalDespawn(this.gameObject);
    }

    private IEnumerator Go()
    {
        float goTime = 0;
        float angValue = 0;
        float distance = 0;

        while (goTime < 5f)
        {
            float x = Mathf.Cos(angValue * Mathf.Deg2Rad * 100f);
            float y = Mathf.Sin(angValue * Mathf.Deg2Rad * 100f);

            float angle = Mathf.Atan2(-y, -x) * Mathf.Rad2Deg;

            _dir = new Vector3(x, y, 0);

            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

            goTime += Time.deltaTime;
            angValue += MoveSpeed * Time.deltaTime;
            distance += Time.deltaTime;

            _rigid.MovePosition(_originalPos + _dir * (Offset + distance));

            yield return null;
        }

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
