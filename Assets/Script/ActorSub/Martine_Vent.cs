using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Martine_Vent : MonoBehaviour, IPunObservable
{
    [SerializeField] private Animator _animator;

    private bool _isOn = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isOn);
        }
        else
        {
            _isOn = (bool)stream.ReceiveNext();
        }

        if (_isOn)
        {
            ActiveVent();
            _isOn = false;
            Debug.Log("Ang");
        }
    }

    public void OnVent()
    {
        _isOn = true;
    }

    public void ActiveVent()
    {
        _animator.Rebind();
        _animator.Play("Vent");
    }
}
