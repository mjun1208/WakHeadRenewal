using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks
{
    [SerializeField] private Team _team;
    private int _hp;
}
