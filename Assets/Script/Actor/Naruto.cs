using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naruto : Actor
{
    private bool _isSkill_2KeyDown = false;
    
    private enum RasenganState
    {
        Ready,
        Charging,
        Shoot
    }

    private RasenganState _rasenganState;

    protected override bool Skill_2Input()
    {        
        if (Input.GetKey(KeyCode.C))
        {
            _isSkill_2KeyDown = true;
        }
        else
        {
            _isSkill_2KeyDown = false;
        }

        return base.Skill_2Input();
    }
    
    public override void OnSkill_1()
    {
        var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", this.transform.position, Quaternion.identity, true);
        var newDummy = Global.PoolingManager.LocalSpawn("Naruto_Dummy", this.transform.position, Quaternion.identity, true);
    }

    public override void OnSkill_2()
    {
        switch (_rasenganState)
        {
            case RasenganState.Ready:
                {
                    _rasenganState = RasenganState.Charging;
                    
                    OnSkill_2();
                    
                    break;
                }
            case RasenganState.Charging:
                {
                    isSkill_2 = true;
                    
                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ChargingRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                    }
                    
                    if (isSkill_2)
                    {
                        _rasenganState = RasenganState.Shoot;
                    }
                    break;
                }
            case RasenganState.Shoot:
                {
                    isSkill_2 = true;

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ShootRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                    }

                    if (isSkill_2)
                    {
                        _rasenganState = RasenganState.Ready;
                    }
                    break;
                }
        }
    }
    
    private IEnumerator ChargingRasengan()
    {
        IsDoingSkill = true;

        _animator.SetBool("IsCharging", true);

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_1"))
        {
            yield return null;
        }
        
        _animator.SetBool("IsSkill_2", false);

        yield return null;

        while (_isSkill_2KeyDown)
        {
            yield return null;
        }

        IsDoingSkill = false;
        isSkill_1 = false;
        isSkill_2 = false;
        
        _animator.SetBool("IsCharging", false);
        _animator.SetBool("IsCharged", true);

        OnSkillCoroutine = null;
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
        _animator.SetBool("IsCharged", false);

        OnSkillCoroutine = null;
    }
}
