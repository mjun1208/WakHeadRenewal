using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAttackRange : AttackRange
{
    [SerializeField] private GameObject _myParent;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        _myParent.SendMessage("OnTriggerEnter2D", collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        _myParent.SendMessage("OnTriggerExit2D", collision);
    }
}
