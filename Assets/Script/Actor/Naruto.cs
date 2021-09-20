using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naruto : Actor
{
    private enum RasenganState
    {
        Ready,
        Charging,
        Shoot
    }

    private RasenganState _rasenganState;

    protected override void OnSkill_2()
    {
        switch (_rasenganState)
        {
            case RasenganState.Ready:
                {
                    base.OnSkill_1();

                    if (_isSkill_1)
                    {
                    }
                    break;
                }
            case RasenganState.Charging:
                {
                    base.OnSkill_1();

                    if (_isSkill_1)
                    {
                    }
                    break;
                }
            case RasenganState.Shoot:
                {
                    base.OnSkill_1();

                    if (_isSkill_1)
                    {
                    }
                    break;
                }
        }
    }


}
