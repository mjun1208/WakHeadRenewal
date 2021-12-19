using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using WakHead;

public class Storm : MonoBehaviourPun
{
    [SerializeField] private AttackRange _attackRange;
    [SerializeField] private bool _isRight = false;
    private float _attackDelay = 0f;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _attackDelay += Time.deltaTime;

            if (_attackDelay > 0.5f)
            {
                _attackDelay = 0f;
                _attackRange.Attack(targetEntity =>
                    {
                        targetEntity?.Damaged(this.transform.position, 1, Team.None, "SansSkill_2Effect", 0);
                    }, Team.None);
            }
        }

        if (Mathf.Abs(this.transform.position.x) > 7.5f)
        {
            this.transform.position += (_isRight ? Vector3.right : Vector3.left) * Time.deltaTime;
        }
    }
}
