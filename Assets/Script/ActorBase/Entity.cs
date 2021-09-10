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

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }

    public void KnockBack(Vector3 dir, float power, float stunTime)
    {
        photonView.RPC("KnockBackRPC", RpcTarget.All, dir, power, stunTime);
    }

    [PunRPC]
    public void KnockBackRPC(Vector3 dir, float power, float stunTime)
    {
        StartCoroutine(OnKnockBack(dir, power, stunTime));
    }

    public void Grab(Vector3 targetPostion, float grabSpeed)
    {
        photonView.RPC("GrabRPC", RpcTarget.All, targetPostion, grabSpeed);
    }

    [PunRPC]
    public void GrabRPC(Vector3 targetPostion, float grabSpeed)
    {
        StartCoroutine(OnGrab(targetPostion, grabSpeed));
    }

    public void Stun(float stunTime)
    {
        photonView.RPC("StunRPC", RpcTarget.All, stunTime);
    }

    [PunRPC]
    public void StunRPC(float stunTime)
    {
        StartCoroutine(OnStun(stunTime));
    }

    private IEnumerator OnKnockBack(Vector3 dir, float power, float stunTime)
    {
        if (stunTime > 0)
        {
            _stun = true;
        }

        _rigid.MovePosition(this.transform.position + dir * power * Time.deltaTime);

        OnDamage();

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

    public void OnDamage()
    {
        Global.PoolingManager.LocalSpawn("Blood", this.transform.position, this.transform.rotation);
    }
}
