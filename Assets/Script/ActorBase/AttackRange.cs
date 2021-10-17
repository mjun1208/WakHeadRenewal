using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private GameObject _ownerObject;
    public List<GameObject> CollidedObjectList { get; private set; } = new List<GameObject>();

    private void OnEnable()
    {
        CollidedObjectList.Clear();
    }

    private void OnDisable()
    {
        CollidedObjectList.Clear();
    }

    public void SetOwner(GameObject owner)
    {
        _ownerObject = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ownerObject == null && !CollidedObjectList.Contains(collision.gameObject))
        {
            if (collision.GetComponent<Entity>() != null)
            {
                CollidedObjectList.Add(collision.gameObject);
            }

            return;
        }

        if (!_ownerObject.Equals(collision.gameObject) && !CollidedObjectList.Contains(collision.gameObject))
        {
            if (collision.GetComponent<Entity>() != null)
            {
                CollidedObjectList.Add(collision.gameObject);
            }
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
