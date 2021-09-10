using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private GameObject _onwerObject;
    public List<GameObject> CollidedObjectList { get; private set; } = new List<GameObject>();

    private void OnEnable()
    {
        CollidedObjectList.Clear();
    }

    private void OnDisable()
    {
        CollidedObjectList.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_onwerObject.Equals(collision.gameObject) && !CollidedObjectList.Contains(collision.gameObject))
        {
            CollidedObjectList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollidedObjectList.Contains(collision.gameObject))
        {
            CollidedObjectList.Remove(collision.gameObject);
        }
    }
}
