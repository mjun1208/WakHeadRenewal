using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickManager : MonoBehaviour
{
    [SerializeField] private SimpleActor _blueActor;
    [SerializeField] private SimpleActor _redActor;

    public void Blue_Select(int index)
    {
        _blueActor.Select(index);
    }

    public void Red_Select(int index)
    {
        _redActor.Select(index);
    }
}
