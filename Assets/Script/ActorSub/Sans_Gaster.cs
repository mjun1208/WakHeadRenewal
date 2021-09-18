using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Gaster : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _blastPivot;

    private GameObject owner;
    private Vector3 _originalScale;
    private Vector3 _dir;

    public const float GasterXOffset = 2f;
    public const float GasterYOffset = 0.2f;
    public const float ChargeOffset = 0.2f;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(GameObject owner, Vector3 pos, Vector3 dir)
    {
        this.transform.position = pos + new Vector3(GasterXOffset * dir.x, GasterYOffset);

        float rotationScale = _originalScale.x * dir.x;
        this.transform.localScale = new Vector3(rotationScale, _originalScale.y, _originalScale.z);

        _dir = dir;
    }

    public void OnBlast()
    {
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
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("FireBlastRPC", RpcTarget.All);
    }

    [PunRPC]
    private void FireBlastRPC()
    {
        var newBlast = Global.PoolingManager.LocalSpawn("Sans_Gaster_Blast", this.transform.position, Quaternion.identity, true);

        newBlast.GetComponent<Sans_Gaster_Blast>().SetInfo(this.photonView, this.gameObject, _blastPivot.transform.position, _dir);
    }
}
