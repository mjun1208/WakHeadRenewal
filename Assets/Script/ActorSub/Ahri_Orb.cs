using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri_Orb : ActorSub
{
    [SerializeField] private TrailRenderer _trail;

    public const float BackSpeed = 15f;

    private List<GameObject> _collidedObjectList = new List<GameObject>();

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _trail.Clear();

        _collidedObjectList.Clear();

        _moveSpeed = Constant.AHRI_ORB_MOVE_SPEED;

        StartCoroutine(Go());
    }

    private void Update()
    {
        if (_attackRange.CollidedObjectList.Count > 0)
        {
            foreach (var targetObject in _attackRange.CollidedObjectList)
            {
                if (!_collidedObjectList.Contains(targetObject))
                {
                    OnDamage(targetObject.GetComponent<Entity>());
                    _collidedObjectList.Add(targetObject);
                }
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

    protected override IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < 0.5f)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * _moveSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(Back());
    }

    private IEnumerator Back()
    {
        _collidedObjectList.Clear();

        float ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

        while (ownerDistance > 0.2f)
        {
            _dir = this.transform.position - _owner.transform.position;
            _rigid.MovePosition(this.transform.position - _dir.normalized * BackSpeed * Time.deltaTime);

            ownerDistance = Vector3.Distance(this.transform.position, _owner.transform.position);

            yield return null;
        }

        _trail.Clear();

        Destroy();
    }
}
