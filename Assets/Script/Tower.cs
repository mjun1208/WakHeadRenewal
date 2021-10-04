using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks
{
    [SerializeField] private Team _team;
    private int _hp;

    private void Awake()
    {
        switch (_team)
        {
            case Team.BLUE:
                {
                    Global.instance.SetBlueTower(this);
                    break;
                }
            case Team.RED:
                {
                    Global.instance.SetRedTower(this);
                    break;
                }
        }
    }
}
