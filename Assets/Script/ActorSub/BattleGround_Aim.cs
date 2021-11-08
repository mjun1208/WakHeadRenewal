using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BattleGround_Aim : ActorSub
{
    [SerializeField] private GameObject _aimObject;
    [SerializeField] private Animator _animator;

    private const float AimSpeed = 10f;

    private int _shootCount = 5;

    public override void SetInfo(PhotonView ownerPhotonView, GameObject owner, Vector3 dir)
    {
        base.SetInfo(ownerPhotonView, owner, dir);

        _aimObject.transform.position = Vector3.zero;

        _shootCount = 5;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _aimObject.transform.position += Vector3.right * AimSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _aimObject.transform.position += Vector3.left * AimSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _aimObject.transform.position += Vector3.up * AimSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _aimObject.transform.position += Vector3.down * AimSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.C))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        _animator.Play("Aim_Shoot");
    }

    public void ReduceShootCount()
    {
        if (!_ownerPhotonView.IsMine)
        {
            return;
        }

        _shootCount--;

        if (_shootCount <= 0)
        {
            Destroy();
        }
    }
}
