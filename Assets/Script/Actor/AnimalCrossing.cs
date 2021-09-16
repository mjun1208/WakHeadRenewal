﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCrossing : Actor
{
    [SerializeField] private AnimalCrossing_Fish myFish;

    private bool _isCasting = false;
    private bool _isBite = false;
    private IEnumerator CastingCoroutine = null;

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

    protected override void OnSkill_1()
    {
        if (!_isCasting)
        {
            // 캐스팅

            base.OnSkill_1();

            myFish.SelectRandomFish();
            photonView.RPC("SetFish", RpcTarget.All, myFish.GetMyFishIndex());
            photonView.RPC("SetActiveFish", RpcTarget.All, false);

            if (_isSkill_1)
            {
                if (_onSkillCoroutine != null)
                {
                    StopCoroutine(_onSkillCoroutine);
                    _onSkillCoroutine = null;
                }

                _isCasting = _isSkill_1;

                CastingCoroutine = Casting();
                StartCoroutine(CastingCoroutine);
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


                if (_onSkillCoroutine == null)
                {
                    _onSkillCoroutine = Fishing();
                    StartCoroutine(_onSkillCoroutine);
                }
            }
        }
    }

    private void Skill_1Cancle()
    {
        if (!_isDoingSkill && _isCasting)
        {
            if (CastingCoroutine != null)
            {
                StopCoroutine(CastingCoroutine);
                CastingCoroutine = null;
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

        _isDoingSkill = false;

        yield return new WaitForSeconds(Random.Range(0.1f, 3f));

        _animator.SetBool("IsSkill_1_1", true);
        _animator.SetBool("IsSkill_1", false);

        _isBite = true;

        photonView.RPC("SetActiveFish", RpcTarget.All, false);

        CastingCoroutine = null;
    }

    private IEnumerator Fishing()
    {
        _isDoingSkill = true;

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

        _isDoingSkill = false;
   
        _isBite = false;

        photonView.RPC("SetActiveFish", RpcTarget.All, false);

        _onSkillCoroutine = null;
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
