using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class ChildAttackRange : AttackRange
    {
        [SerializeField] private AttackRange _myParentAttackRange;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (_myParentAttackRange != null)
            {
                _myParentAttackRange.SendMessage("OnTriggerEnter2D", collision);
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (_myParentAttackRange != null)
            {
                _myParentAttackRange.SendMessage("OnTriggerExit2D", collision);
            }
        }
    }
}