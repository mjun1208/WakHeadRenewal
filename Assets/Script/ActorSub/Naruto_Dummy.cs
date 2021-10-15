using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Naruto_Dummy : ActorSub
{
    [SerializeField] private Animator _animator;

    public void Attack(bool isAttack)
    {
        _animator.SetBool("IsAttack", isAttack);
    }
    
    public void SetAnimationParameter(string name, bool isTrue)
    {
        _animator.SetBool(name, isTrue);
    }

    public void Rasengan()
    {
    }
}
