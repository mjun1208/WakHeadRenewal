using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ahri : Actor
{
    [SerializeField] private GameObject OrbPrefab;
    [SerializeField] private GameObject HeartPrefab;

    private GameObject MyOrb;

    private float _rushSpeed = 15f;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_isDoingSkill && _isSkill_2)
        {
            SpiritRush();
        }
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var newHeart = Global.PoolingManager.Spawn("Heart", this.transform.position, Quaternion.identity, (heart =>
        {
            heart.GetComponent<Heart>().SetInfo(GetAttackDir());
        }));

        //newHeart.GetComponent<Heart>().SetInfo(GetAttackDir());
    }

    protected override void Attack()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
            !_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            return;
        }

        _animator.SetBool("IsAttack", false);

        if (_isAttackInput)
        {
            if (MyOrb == null)
            {
                MyOrb = Global.PoolingManager.Spawn("Orb", this.transform.position, Quaternion.identity);
                MyOrb.SetActive(false);
            }

            if (!MyOrb.activeSelf)
            {
                base.Attack();
                MyOrb.SetActive(true);
                MyOrb.GetComponent<Orb>().SetInfo(this.gameObject, GetAttackDir());
            }
        }
    }

    private void SpiritRush()
    {
        _rigid.MovePosition(transform.position + GetAttackDir() * _rushSpeed * Time.deltaTime);
    }
}
