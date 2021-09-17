using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Rigidbody2D _rigid;

    public bool _knockBack = false;
    public bool _grab = false;

    protected bool _stun = false;

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

        OnCrownControl();

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

        OnCrownControl();

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

        OnCrownControl();

        currentCrownControl = OnStun(stunTime);

        StartCoroutine(currentCrownControl);
    }

    private IEnumerator OnKnockBack(Vector3 dir, float power, float stunTime)
    {
        if (stunTime > 0)
        {
            _stun = true;
        }

        OnDamage();

        var targetPosition = this.transform.position + dir * power;

        float distance = float.MaxValue;

        while (distance > 0.3f)
        {
            distance = Vector3.Distance(this.transform.position, targetPosition);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 15f);

            yield return null;
        }

        yield return new WaitForSeconds(stunTime);

        _stun = false;
    }

    private IEnumerator OnGrab(Vector3 targetPostion, float grabSpeed)
    {
        _stun = true;

        float distance = float.MaxValue;

        while (distance > 0.1f)
        {
            distance = Vector3.Distance(this.transform.position, targetPostion);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPostion, Time.deltaTime * grabSpeed);

            yield return null;
        }

        OnDamage();

        yield return null;

        _stun = false;
    }

    private IEnumerator OnStun(float stunTime)
    {
        _stun = true;

        yield return new WaitForSeconds(stunTime);

        _stun = false;
    }

    public void Damaged()
    {
        photonView.RPC("OnDamageRPC", RpcTarget.All);
    }

    [PunRPC]
    public void OnDamageRPC()
    {
        OnDamage();
    }

    public void OnDamage()
    {
        var randomPos = (Vector3)Random.insideUnitCircle * 0.5f;

        Global.PoolingManager.LocalSpawn("HitEffect", this.transform.position + randomPos, this.transform.rotation , true);
    }
}
