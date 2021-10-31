using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naruto : Actor
{
    private List<Naruto_Dummy> _dummieList = new List<Naruto_Dummy>();
    private bool _isSkill_2KeyDown = false;

    private int _chargingGauge = 0;

    private enum RasenganState
    {
        Ready,
        Charging,
        Shoot
    }

    private RasenganState _rasenganState;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
            stream.SendNext(_chargingGauge);
        }
        else
        {
            _chargingGauge = (int)stream.ReceiveNext();
        }
    }

    protected override void Update()
    {
        foreach (var dummy in _dummieList)
        {
            dummy.SetDir(GetAttackDir());
        }

        if (!photonView.IsMine)
        {
            return;
        }

        base.Update();
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _attackRange.Attack(targetEntity =>
        {
            targetEntity.KnockBack(5, GetAttackDir(), 1.5f, 0);
        });
    }

    protected override void Attack()
    {
        base.Attack();
        SetDummyAnimation("IsAttack", _isAttackInput);
    }
    
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
        if (_rasenganState == RasenganState.Shoot)
        {
            return;
        }

        photonView.RPC("SummonDummyRPC", RpcTarget.All);
    }

    public override void OnSkill_2()
    {
        switch (_rasenganState)
        {
            case RasenganState.Ready:
                {
                    _rasenganState = RasenganState.Charging;
                    
                    OnSkill_2();
                    SetDummyAnimation("IsSkill_2", true);

                    break;
                }
            case RasenganState.Charging:
                {
                    _chargingGauge = 0;

                    IsSkill_2 = true;

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ChargingRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                        SetDummyAnimation("IsSkill_2", true);
                    }
                    
                    if (IsSkill_2)
                    {
                        _rasenganState = RasenganState.Shoot;
                    }
                    break;
                }
            case RasenganState.Shoot:
                {
                    IsSkill_2 = true;

                    if (OnSkillCoroutine == null)
                    {
                        IsDoingSkill = true;
                        OnSkillCoroutine = ShootRasengan();
                        StartCoroutine(OnSkillCoroutine);

                        _animator.SetBool("IsSkill_2", true);
                        SetDummyAnimation("IsSkill_2", true);
                    }

                    if (IsSkill_2)
                    {
                        _rasenganState = RasenganState.Ready;
                    }
                    break;
                }
        }
    }

    public void Charging()
    {
        _chargingGauge += 1;
    }

    private IEnumerator ChargingRasengan()
    {
        IsDoingSkill = true;

        _animator.SetBool("IsCharging", true);
        SetDummyAnimation("IsCharging", true);

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_1"))
        {
            yield return null;
        }
        
        _animator.SetBool("IsSkill_2", false);
        SetDummyAnimation("IsSkill_2", false);

        yield return null;

        while (_isSkill_2KeyDown && _chargingGauge < 3)
        {
            yield return null;
        }

        IsDoingSkill = false;
        IsSkill_1 = false;
        IsSkill_2 = false;
        
        _animator.SetBool("IsCharging", false);
        _animator.SetBool("IsCharged", true);
        
        SetDummyAnimation("IsCharging", false);
        SetDummyAnimation("IsCharged", true);

        OnSkillCoroutine = null;
    }

    private IEnumerator ShootRasengan()
    {
        IsDoingSkill = true;

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_2_2"))
        {
            yield return null;
        }

        SetDummyAnimation("IsSkill_2", false);
        SetDummyAnimation("IsCharged", false);

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        IsDoingSkill = false;
        IsSkill_1 = false;
        IsSkill_2 = false;

        _animator.SetBool("IsSkill_2", false);
        _animator.SetBool("IsCharged", false);
        
        SetDummyAnimation("IsSkill_2", false);
        SetDummyAnimation("IsCharged", false);

        OnSkillCoroutine = null;
    }
    
    public void Rasengan()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("RasenganRPC", RpcTarget.All);
    }

    [PunRPC]
    public void RasenganRPC()
    {
        var newRasengan = Global.PoolingManager.LocalSpawn("Naruto_Rasengan", this.transform.position, Quaternion.identity, true);
        newRasengan.GetComponent<Naruto_Rasengan>().SetInfo(this.photonView, this.gameObject, this.transform.position, GetAttackDir(), _chargingGauge);
        
        foreach (var dummy in _dummieList)
        {
            var newDummyRasengan = Global.PoolingManager.LocalSpawn("Naruto_Rasengan", dummy.transform.position, Quaternion.identity, true);
            newDummyRasengan.GetComponent<Naruto_Rasengan>().SetInfo(this.photonView, this.gameObject, dummy.transform.position, GetAttackDir(), _chargingGauge);
        }
    }

    [PunRPC]
    public void SummonDummyRPC()
    {
        var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", this.transform.position, Quaternion.identity, true);
        var newDummy = Global.PoolingManager.LocalSpawn("Naruto_Dummy", this.transform.position, Quaternion.identity, true);

        newDummy.GetComponent<Naruto_Dummy>().SetInfo(this.photonView, this.gameObject, this.transform.position);
        newDummy.GetComponent<Naruto_Dummy>().SetDir(GetAttackDir());

        _dummieList.Add(newDummy.GetComponent<Naruto_Dummy>());
    }

    private void SetDummyAnimation(string name, bool isTrue)
    {
        // foreach (var dummy in _dummieList)
        // {
        //     dummy.SetAnimationParameter(name, isTrue);
        // }

        photonView.RPC("SetDummyAnimationRPC", RpcTarget.All, name, isTrue);
    }

    [PunRPC]
    public void SetDummyAnimationRPC(string name, bool isTrue)
    {
        foreach (var dummy in _dummieList)
        {
            dummy.SetAnimationParameter(name, isTrue);
        }
    }

    protected override void Dead()
    {
        base.Dead();
        _rasenganState = RasenganState.Ready;
        _isSkill_2KeyDown = false;
        _chargingGauge = 0;
    }
}
