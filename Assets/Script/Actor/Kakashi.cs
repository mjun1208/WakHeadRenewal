using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi : Actor
{
    [SerializeField] private Material _copyMaterial;

    private GameObject _copyActor = null;
    private Actor _copyActorScprit = null;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        var targetList = _skill_1Range.CollidedObjectList;

        foreach (var target in targetList)
        {
            var targetEntity = target.GetComponent<Entity>();
            targetEntity.KnockBack(GetAttackDir(), 1f, 0);
        }
    }

    protected override void Active_Skill_2()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    public override void OnSkill_2()
    {
        if (OnSkillCoroutine != null)
        {
            return;
        }

        photonView.RPC("Sharingan", RpcTarget.All);

        if (string.IsNullOrEmpty(Global.instance.EnemyActorName))
        {
            return;
        }

        DestroyCopiedActor();

        _copyActor = PhotonNetwork.Instantiate(Global.instance.EnemyActorName, this.transform.position, Quaternion.identity);
        _copyActorScprit = _copyActor.GetComponent<Actor>();

        photonView.RPC("ActorCopy", RpcTarget.All, _copyActor.GetPhotonView().ViewID, true);

        _copyActor.transform.position = this.transform.position;
        _copyActor.transform.localScale = this.transform.localScale;

        OnSkillCoroutine = DoCopyActor();
        StartCoroutine(OnSkillCoroutine);
    }

    private IEnumerator DoCopyActor()
    {
        float lifetime = 0f;

        while (lifetime < 5f)
        {
            lifetime += Time.deltaTime;
            yield return null;
        }

        ReturnKakashi();

        OnSkillCoroutine = null;
    }

    private void ReturnKakashi()
    {
        base.Start();

        photonView.RPC("ActorCopy", RpcTarget.All, _copyActor.GetPhotonView().ViewID, false);

        _copyActorScprit = null;

        this.transform.position = _copyActor.transform.position;
        this.transform.localScale = _copyActor.transform.localScale;
    }

    private void DestroyCopiedActor()
    {
        if (_copyActor != null)
        {
            PhotonNetwork.Destroy(_copyActor);
        }
    }

    [PunRPC]
    public void ActorCopy(int actorID, bool isCopy)
    {
        _copyActor = PhotonView.Find(actorID).gameObject;

        _copyActor.SetActive(isCopy);
        _copyActor.GetComponent<SpriteRenderer>().material = _copyMaterial;
        this._renderer.enabled = !isCopy;
    }

    [PunRPC]
    public void Sharingan()
    {
        var newEye = Global.PoolingManager.LocalSpawn("Kakashi_EyeCanvas", this.transform.position, Quaternion.identity, true);
    }
}
