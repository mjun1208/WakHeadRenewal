using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine_Vent : Summoned
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrow;

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        MaxHP = 5;
        HP = MaxHP;
    }

    public void OnVent()
    {
        photonView.RPC("ActiveVent", RpcTarget.All);
    }

    [PunRPC]
    private void ActiveVent()
    {
        _animator.Rebind();
        _animator.Play("Vent");
    }

    public void Select(bool isSelect)
    {
        _arrow.SetActive(isSelect);
    }
}
