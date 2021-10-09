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

    public override void OnSkill_1()
    {
        var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", this.transform.position, Quaternion.identity, true);
        var newDummy = Global.PoolingManager.Spawn("Naruto_Dummy", this.transform.position, Quaternion.identity);
    }

    public override void OnSkill_2()
    {
        switch (_rasenganState)
        {
            case RasenganState.Ready:
                {
                    base.OnSkill_2();

                    if (isSkill_2)
                    {
                        _rasenganState = RasenganState.Charging;
                    }
                    break;
                }
            case RasenganState.Charging:
                {
                    if (isSkill_2)
                    {
                        _rasenganState = RasenganState.Shoot;
                    }
                    break;
                }
            case RasenganState.Shoot:
                {
                    isSkill_2 = true;

                    Debug.Log("vow");

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ShootRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                        _animator.SetBool("IsCharging", true);
                    }

                    if (isSkill_2)
                    {
                        _rasenganState = RasenganState.Ready;
                    }
                    break;
                }
        }
    }

    private IEnumerator ShootRasengan()
    {
        IsDoingSkill = true;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_2"))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        IsDoingSkill = false;
        isSkill_1 = false;
        isSkill_2 = false;

        _animator.SetBool("IsSkill_2", false);
        _animator.SetBool("IsCharging", false);

        OnSkillCoroutine = null;
    }
}
