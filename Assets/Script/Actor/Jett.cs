using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jett : Actor
{
    [SerializeField] private GameObject _ghostPivot;
    [SerializeField] private GameObject _operatorTrajectoryPivot;

    private int _shurikenCount = 0;

    protected override void Start()
    {
        base.Start();

        _attackMoveSpeed = 2f;
    }

    protected override void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _isAttack = true;
        }
        else
        {
            _isAttack = false;
        }

        base.Update();
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(_ghostPivot.transform.position, GetAttackDir(), 5f);
        Debug.DrawRay(_ghostPivot.transform.position, GetAttackDir() * 5f, Color.red, 3f);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                continue;
            }

            var entity = hit.transform.GetComponent<Entity>();
            if (entity != null)
            {
                entity.KnockBack(GetAttackDir(), 0.5f, 0);
                break;
            }
        }

        // photonView.RPC("ShootGhost", RpcTarget.All, _isMove);
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        int layerMask = (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Summoned"));
        RaycastHit2D[] hits = Physics2D.RaycastAll(_ghostPivot.transform.position, GetAttackDir(), 20f, layerMask);
        Debug.DrawRay(_ghostPivot.transform.position, GetAttackDir() * 20f, Color.red, 3f);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                continue;
            }

            var entity = hit.transform.GetComponent<Entity>();
            if (entity != null)
            {
                entity.KnockBack(GetAttackDir(), 1f, 0);
            }
        }

        photonView.RPC("ShootOperator", RpcTarget.All);
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _shurikenCount += 10;
    }

    private void ThrowShuriken()
    {
        _shurikenCount--;
    }

    protected override void Attack()
    {
        base.Attack();
        if (_shurikenCount == 0)
        {
            _animator.SetBool("IsAttack", _isAttackInput);

            _animator.SetBool("IsAttack_2_0", false);
            _animator.SetBool("IsAttack_2_1", false);
        }
        else
        {
            _animator.SetBool("IsAttack_2_0", _isAttackInput && _shurikenCount % 2 == 0);
            _animator.SetBool("IsAttack_2_1", _isAttackInput && _shurikenCount % 2 == 1);
        }
    }

    [PunRPC]
    public void ShootOperator()
    {
        var newOperatorTrajectory = Global.PoolingManager.LocalSpawn("OperatorTrajectory", _operatorTrajectoryPivot.transform.position, Quaternion.identity, true);
        newOperatorTrajectory.GetComponent<SpriteRenderer>().flipX = GetAttackDir().x < 0;
    }
}
