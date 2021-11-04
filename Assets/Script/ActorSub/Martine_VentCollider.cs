using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine_VentCollider : MonoBehaviour
{
    [SerializeField] private Martine _martine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _martine.VentColliderEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _martine.VentColliderExit(collision);
    }
}
