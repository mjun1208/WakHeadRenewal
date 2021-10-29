using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Gaster : Summoned
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _blastPivot;

    private int _ownerID;
    private Vector3 _originalScale;
    private bool _isFire = false;

    public const float GasterXOffset = 1f;
    public const float GasterYOffset = 0.2f;
    public const float ChargeOffset = 0.2f;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(int ownerID, GameObject owner, Vector3 pos, Vector3 dir)
    {
        _ownerID = ownerID;

        _owner = owner;

        this.transform.position = pos;

        photonView.RPC("SetInfo", RpcTarget.All, _ownerID, dir);
    }

    [PunRPC]
    private void SetInfo(int ownerID, Vector3 dir)
    {
        _ownerID = ownerID;

        _owner = PhotonView.Find(ownerID).gameObject;

        this.transform.position += new Vector3(GasterXOffset * dir.x, GasterYOffset);

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

        _dir = dir;
    }

    public void OnBlast()
    {
        if (_isFire)
        {
            return;
        }

        _isFire = true;

        photonView.RPC("ActiveBlast", RpcTarget.All);
    }

    [PunRPC]
    private void ActiveBlast()
    {
        _animator.Rebind();
        _animator.Play("Fire");
    }


    public void OnCharging()
    {
        photonView.RPC("ActiveCharging", RpcTarget.All);
    }

    [PunRPC]
    private void ActiveCharging()
    {
        var newCharging = Global.PoolingManager.LocalSpawn("Sans_Charge_Blast", _blastPivot.transform.position + _dir * ChargeOffset, Quaternion.identity, true);
    }


    public void FireBlast()
    {
        var newBlast = Global.PoolingManager.LocalSpawn("Sans_Gaster_Blast", this.transform.position, Quaternion.identity, true);
        newBlast.GetComponent<Sans_Gaster_Blast>().SetInfo(this.photonView, this.gameObject, _owner, _blastPivot.transform.position, _dir);
    }
}
