﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Rigidbody2D _rigid;

    private bool _knockBack = false;
    private bool _grab = false;

    public Action<bool> StunAction;
    public Action CrownControlAction;

    public Team MyTeam { get; protected set; } = Team.None;

    private int _maxHP;
    private int _currentHP;

    public bool IsStun {
        get
        {
            return _isStun;
        }
        protected set
        {
            StunAction?.Invoke(value);
            _isStun = value;
        }
    }

    public int MaxHP
    {
        get
        {
            return _maxHP;
        }
        protected set
        {
            _maxHP = value;
        }
    }

    public int HP
    {
        get
        {
            return _currentHP;
        }
        protected set
        {
            _currentHP = value;
        }
    }

    protected bool _isStun = false;

    private IEnumerator currentCrownControl = null;

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }

    protected virtual void Awake()
    {
        CrownControlAction += OnCrownControl;
    }

    public void SetTeam(Team team)
    {
        MyTeam = team;
    }
    
    public void OnCrownControl()
    {
        if (currentCrownControl != null)
        {
            StopCoroutine(currentCrownControl);
        }

        currentCrownControl = null;
    }

    public void KnockBack(Vector3 dir, float power, float stunTime)
    {
        photonView.RPC("KnockBackRPC", RpcTarget.All, dir, power, stunTime);
    }

    [PunRPC]
    public void KnockBackRPC(Vector3 dir, float power, float stunTime)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        CrownControlAction?.Invoke();

        currentCrownControl = OnKnockBack(dir, power, stunTime);

        StartCoroutine(currentCrownControl);
    }

    public void Grab(Vector3 targetPostion, float grabSpeed)
    {
        photonView.RPC("GrabRPC", RpcTarget.All, targetPostion, grabSpeed);
    }

    [PunRPC]
    public void GrabRPC(Vector3 targetPostion, float grabSpeed)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        CrownControlAction?.Invoke();

        currentCrownControl = OnGrab(targetPostion, grabSpeed);

        StartCoroutine(currentCrownControl);
    }

    public void Stun(float stunTime)
    {
        photonView.RPC("StunRPC", RpcTarget.All, stunTime);
    }

    [PunRPC]
    public void StunRPC(float stunTime)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        CrownControlAction?.Invoke();

        currentCrownControl = OnStun(stunTime);

        StartCoroutine(currentCrownControl);
    }

    private IEnumerator OnKnockBack(Vector3 dir, float power, float stunTime)
    {
        if (stunTime > 0)
        {
            IsStun = true;
        }

        Damaged(this.transform.position);

        var targetPosition = this.transform.position + dir * power;

        float distance = float.MaxValue;

        while (distance > 0.3f)
        {
            distance = Vector3.Distance(this.transform.position, targetPosition);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 15f);

            yield return null;
        }

        yield return new WaitForSeconds(stunTime);

        IsStun = false;
    }

    private IEnumerator OnGrab(Vector3 targetPostion, float grabSpeed)
    {
        IsStun = true;

        float distance = float.MaxValue;

        while (distance > 0.1f)
        {
            distance = Vector3.Distance(this.transform.position, targetPostion);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPostion, Time.deltaTime * grabSpeed);

            yield return null;
        }

        Damaged(this.transform.position);

        yield return null;

        IsStun = false;
    }

    private IEnumerator OnStun(float stunTime)
    {
        IsStun = true;

        yield return new WaitForSeconds(stunTime);

        IsStun = false;
    }

    public void Damaged(Vector3 pos)
    {
        photonView.RPC("OnDamageRPC", RpcTarget.All, pos);
    }

    [PunRPC]
    public void OnDamageRPC(Vector3 pos)
    {
        OnDamage(pos);
    }

    public void OnDamage(Vector3 pos)
    {
        var randomPos = (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f;

        Global.PoolingManager.LocalSpawn("HitEffect", this.transform.position + randomPos, this.transform.rotation , true);
    }
}
