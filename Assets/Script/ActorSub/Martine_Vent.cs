using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine_Vent : Summoned
{
    [SerializeField] private Animator _animator;

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
}
