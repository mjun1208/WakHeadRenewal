using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class Banana_BallCollider : MonoBehaviour
    {
        [SerializeField] private Banana _banana;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _banana.BallColliderEnter(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _banana.BallColliderExit(collision);
        }
    }
}
