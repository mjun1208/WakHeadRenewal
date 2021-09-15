using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Bone : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AttackRange _attackRange;

    public const float X_OFFSET = 3f;

    public void SetInfo(GameObject owner, Vector3 dir)
    {
        _attackRange.SetOwner(owner);

        this.transform.position = owner.transform.position + new Vector3(X_OFFSET * dir.x, 0, 0);

        _animator.Rebind();
        _animator.Play("Vent");

        _attackRange.CollidedObjectList.Clear();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            Global.PoolingManager.LocalDespawn(this.gameObject);
        }
    }

    private void OnDamage()
    {
        foreach (var targetObject in _attackRange.CollidedObjectList)
        {
            targetObject.GetComponent<Entity>().Damaged();
        }
    }
}
