using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi : Actor
{
    [SerializeField] private Material _copyMaterial;

    private GameObject _copyActor = null;
    private Actor _copyActorScript = null;

    private Vector3 _shurikenPosition;
    private Vector3 _shurikenDir;

    private List<Kakashi_Shuriken> _shurikenList = new List<Kakashi_Shuriken>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        base.Update();

        if (_copyActorScript != null)
        {
            var damage = _copyActorScript.MaxHP - _copyActorScript.HP;
            _copyActorScript.ResetHp();
            HP -= damage;
        }
    }

    protected override void Active_Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _shurikenPosition = this.transform.position;
        _shurikenDir = GetAttackDir();

        photonView.RPC("ThrowShuriken", RpcTarget.All, _shurikenPosition, _shurikenDir);
    }

    protected override void Active_Skill_1()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _skill_1Range.Attack(targetEntity =>
        {
            targetEntity.KnockBack(10 ,GetAttackDir(), 1f, 0);
        });
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

        if (string.IsNullOrEmpty(Global.instance.EnemyActorName) || Global.instance.EnemyActorName == Global.instance.MyActorName)
        {
            return;
        }

        DestroyCopiedActor();

        _copyActor = PhotonNetwork.Instantiate(Global.instance.EnemyActorName, this.transform.position, Quaternion.identity);
        _copyActorScript = _copyActor.GetComponent<Actor>();

        _copyActor.transform.position = this.transform.position;
        _copyActor.transform.localScale = this.transform.localScale;

        photonView.RPC("ActorCopy", RpcTarget.All, _copyActor.GetPhotonView().ViewID, true);

        OnSkillCoroutine = DoCopyActor();
        StartCoroutine(OnSkillCoroutine);
    }

    private IEnumerator DoCopyActor()
    {
        float lifetime = 0f;

        while (lifetime < 5f && !_isAttackInput)
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

        _smoothSync.teleport();
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
        if (isCopy)
        {
            _copyActor = PhotonView.Find(actorID).gameObject;
            _copyActor.GetComponent<SpriteRenderer>().material = _copyMaterial;
        }

        _renderer.enabled = !isCopy;
        _collider2D.enabled = !isCopy;

        var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", _copyActor.transform.position, Quaternion.identity, true);
    }

    [PunRPC]
    public void ThrowShuriken(Vector3 shurikenPosition, Vector3 shurikenDir)
    {
        var newShuriken = Global.PoolingManager.LocalSpawn("Kakashi_Shuriken", this.transform.position, Quaternion.identity, true);
        var shurikenScript = newShuriken.GetComponent<Kakashi_Shuriken>();
        shurikenScript.SetInfo(this.photonView, this.gameObject, shurikenPosition, shurikenDir);

        _shurikenList.Add(shurikenScript);
        shurikenScript.DestoryAction += DespawnShuriken;
    }

    public void DespawnShuriken(ActorSub shuriken)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        shuriken.DestoryAction -= DespawnShuriken;

        photonView.RPC("DespawnShurikenRPC", RpcTarget.All, GetShurikenIndex(shuriken as Kakashi_Shuriken));
    }

    private int GetShurikenIndex(Kakashi_Shuriken targetShuriken)
    {
        int index = 0;

        foreach (var shuriken in _shurikenList)
        {
            if (shuriken == targetShuriken)
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    [PunRPC]
    public void DespawnShurikenRPC(int count)
    {
        Debug.Log(count);
        var targetShuriken = _shurikenList[count];

        _shurikenList.Remove(targetShuriken);

        Global.PoolingManager.LocalDespawn(targetShuriken.gameObject);
    }

    [PunRPC]
    public void Sharingan()
    {
        var newEye = Global.PoolingManager.LocalSpawn("Kakashi_EyeCanvas", this.transform.position, Quaternion.identity, true);
    }
}
