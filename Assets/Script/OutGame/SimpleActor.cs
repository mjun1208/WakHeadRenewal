using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActor : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private List<RuntimeAnimatorController> _animatorControllerList;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Confirmed();
        }
    }

    public void Select(int index)
    {
        _animator.runtimeAnimatorController = _animatorControllerList[index];
    }

    public void Confirmed()
    {
        _animator.Play("Attack");
    }

    public void Active_Attack()
    {
    }
}
