﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorGameDataArray
{
    public ActorGameData[] ActorGameDataList;
}

[System.Serializable]
public class ActorGameData
{
    public int ID;
    public string Name;
    public string Artist;
    public string KorName;
}