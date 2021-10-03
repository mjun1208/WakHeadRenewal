using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimpanzee : Entity
{
    private enum ChimpanzeeState
    {
        Move,
        Attack
    }

    private ChimpanzeeState _state = ChimpanzeeState.Move;
    private Entity _targetEntity;
    // private Entity _targetTower;

    public void SetTarget(Entity entity)
    {
        _targetEntity = entity;
    }

    private void Update()
    {
        switch (_state)
        {
            case ChimpanzeeState.Move:
                {
                    break;
                }
            case ChimpanzeeState.Attack:
                {
                    break;
                }
        }
    }

    private void Move()
    {
        if (_targetEntity == null)
        {

        }
    }

    private void Attack()
    {

    }
}
