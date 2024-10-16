﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class OccupiedCollider : MonoBehaviour
    {
        public Team MyTeam { get; private set; } = Team.None;
        public bool IsWork { get; set; } = true;

        public void SetTeam(Team team)
        {
            MyTeam = team;
        }
    }
}
