using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private GameObject _ownerObject;
    public List<GameObject> CollidedObjectList { get; private set; } = new List<GameObject>();
    public List<GameObject> CollidedSummonedObjectList { get; private set; } = new List<GameObject>();

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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ownerObject == null)
        {
            AddCollidedObject(collision.gameObject);
            return;
        }

        if (!_ownerObject.Equals(collision.gameObject))
        {
            AddCollidedObject(collision.gameObject);
        }
    }

    private void AddCollidedObject(GameObject gameObject)
    {
        if (!CollidedObjectList.Contains(gameObject))
        {
            if (gameObject.GetComponent<Entity>() != null)
            {
                CollidedObjectList.Add(gameObject);
            }
        }

        if (!CollidedSummonedObjectList.Contains(gameObject))
        {
            if (gameObject.GetComponent<Summoned>() != null)
            {
                CollidedSummonedObjectList.Add(gameObject);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (CollidedObjectList.Contains(collision.gameObject))
        {
            CollidedObjectList.Remove(collision.gameObject);
        }

        if (CollidedSummonedObjectList.Contains(collision.gameObject))
        {
            CollidedSummonedObjectList.Remove(collision.gameObject);
        }
    }
}
