using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Banana_TrampolineCollider : MonoBehaviour
    {
        [SerializeField] private Banana _banana;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _banana.TrampolineColliderEnter(other);
        }
    }
}
