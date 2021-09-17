using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sans_Gaster : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator _animator;

    private GameObject owner;
    private Vector3 _originalScale;
    private Vector3 _dir;

    public const float BlastOffset = 1.5f;

    private void Awake()
    {
        _originalScale = this.transform.localScale;
    }

    public void SetInfo(GameObject owner, Vector3 pos, Vector3 dir)
    {
        this.transform.position = pos + new Vector3(BlastOffset * dir.x, 0);

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

        newBlast.GetComponent<Sans_Gaster_Blast>().SetInfo(this.photonView, this.gameObject, _dir);
    }
}
