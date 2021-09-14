using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri_Orb : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private AttackRange _attackRange;
    private GameObject _owner;
    private Vector3 _dir;

    public const float GoSpeed = 10f;
    public const float BackSpeed = 15f;

    private List<GameObject> _collidedObjectList = new List<GameObject>();

    public void SetInfo(GameObject owner, Vector3 dir)
    {
        _attackRange.SetOwner(owner);
        this.transform.position = owner.transform.position;

        _trail.Clear();

        _owner = owner;
        _dir = dir;

        _collidedObjectList.Clear();

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

    private void OnDamage(Entity entity)
    {
        entity.Damaged();
    }

    private IEnumerator Go()
    {
        float goTime = 0;

        while (goTime < 0.5f)
        {
            goTime += Time.deltaTime;
            _rigid.MovePosition(this.transform.position + _dir * GoSpeed * Time.deltaTime);

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
        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
