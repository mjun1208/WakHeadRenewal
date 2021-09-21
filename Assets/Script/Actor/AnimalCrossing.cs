﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCrossing : Actor
{
    [SerializeField] private AnimalCrossing_Fish myFish;

    private bool _isCasting = false;
    private bool _isBite = false;
    private IEnumerator _castingCoroutine = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_isMoveInput)
        {
            Skill_1Cancle();
        }
    }

    protected override void ForceStop(bool isStun)
    {
        base.ForceStop(isStun);
        StopCoroutine(_castingCoroutine);
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetList = _attackRange.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.KnockBack(GetAttackDir(), 0.5f, 0);
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("ShootFish", RpcTarget.All, myFish.GetMyFishIndex());
    }

    public override void OnSkill_1()
    {
        if (!_isCasting)
        {
            // 캐스팅

            base.OnSkill_1();

            _isCasting = isSkill_1;

            if (isSkill_1)
            {
                if (OnSkillCoroutine != null)
                {
                    StopCoroutine(OnSkillCoroutine);
                    OnSkillCoroutine = null;
                }

                _castingCoroutine = Casting();
                StartCoroutine(_castingCoroutine);
            }
        }
        else
        {
            if (_isBite)
            {
                _isCasting = false;

                // 물고기가 물었을 때

                _animator.SetBool("IsSkill_1_2", true);
                _animator.SetBool("IsSkill_1_1", false);

                if (OnSkillCoroutine == null)
                {
                    OnSkillCoroutine = Fishing();
                    StartCoroutine(OnSkillCoroutine);
                }
            }
        }
    }

    private void Skill_1Cancle()
    {
        if (!IsDoingSkill && _isCasting)
        {
            if (_castingCoroutine != null)
            {
                StopCoroutine(_castingCoroutine);
                _castingCoroutine = null;
            }

            _isCasting = false;

            photonView.RPC("SetActiveFish", RpcTarget.All, false);

            SkillCancle();
            _animator.SetBool("IsSkill_1_1", false);
            _animator.SetBool("IsSkill_1_2", false);
        }
    }

    private IEnumerator Casting()
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_1"))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        IsDoingSkill = false;

        yield return new WaitForSeconds(Random.Range(0.1f, 3f));

        _animator.SetBool("IsSkill_1_1", true);
        _animator.SetBool("IsSkill_1", false);

        _isBite = true;

        photonView.RPC("SetActiveFish", RpcTarget.All, false);

        _castingCoroutine = null;
    }

    private IEnumerator Fishing()
    {
        IsDoingSkill = true;

        myFish.SelectRandomFish();
        photonView.RPC("SetFish", RpcTarget.All, myFish.GetMyFishIndex());

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Skill_1_2"))
        {
            yield return null;
        }

        yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        // end

        _animator.SetBool("IsSkill_1_2", false);

        IsDoingSkill = false;
   
        _isBite = false;

        photonView.RPC("SetActiveFish", RpcTarget.All, false);

        OnSkillCoroutine = null;
    }

    [PunRPC]
    private void SetActiveFish(bool isActive)
    {
        myFish.SetActive(isActive);
    }

    [PunRPC]
    private void SetFish(int index)
    {
        myFish.SelectFish(index);
    }

    [PunRPC]
    private void ShootFish(int index)
    {
        var newFish = Global.PoolingManager.LocalSpawn("AnimalCrossing_FishBullet", this.transform.position, Quaternion.identity, true);

        newFish.GetComponent<AnimalCrossing_FishBullet>().SetInfo(this.photonView, this.gameObject, GetAttackDir(), index);
    }
}
