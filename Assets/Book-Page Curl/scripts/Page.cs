using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private List<GameObject> _actorInfo = new List<GameObject>();

    public int Index
    {
        get
        {
            return _index;
        }
        set
        {
            SetIndex(value);
        }
    }

    private int _index { get; set; } = -1;

    private void SetIndex(int index)
    {
        _index = index;

        foreach (var actor in _actorInfo)
        {
            actor.SetActive(false);
        }

        if (_index != -1)
        {
            _actorInfo[_index].SetActive(true);
        }
    }
}
