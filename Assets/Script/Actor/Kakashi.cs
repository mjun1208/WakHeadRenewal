using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
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

            if (_copyActorScript != null)
            {
                var damage = _copyActorScript.MaxHP - _copyActorScript.HP;
                _copyActorScript.ResetHp();
                HP -= damage;
            }
            else
            {
                base.Update();   
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

            _skill_1Range.Attack(targetEntity => { targetEntity.KnockBack(10, GetAttackDir(), 1f, 0, MyTeam, 
                "KakashiSkill_1Effect",0, Random.Range(0, 2) == 0); }, MyTeam);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();
        }

        public override void PlaySkill_1Sound()
        {
            Global.SoundManager.Play("Kakashi_Skill_1_Sound", this.transform.position);
        }
        
        public override void PlaySkill_2Sound()
        {
            Global.SoundManager.Play("Kakashi_Skill_2_Sound", this.transform.position);
        }

        [PunRPC]
        public void PlaySkill_2SoundRPC()
        {
            PlaySkill_2Sound();
        }

        public override void OnSkill_2()
        {
            if (OnSkillCoroutine != null)
            {
                return;
            }

            Skill_2_Delay = Skill_2_CoolTime;

            photonView.RPC("PlaySkill_2SoundRPC", RpcTarget.All);
            photonView.RPC("Sharingan", RpcTarget.All);

            if (string.IsNullOrEmpty(Global.instance.EnemyActorName) ||
                Global.instance.EnemyActorName == Global.instance.MyActorName)
            {
                return;
            }

            DestroyCopiedActor();

            _copyActor = PhotonNetwork.Instantiate(Global.instance.EnemyActorName, this.transform.position,
                Quaternion.identity);
            _copyActorScript = _copyActor.GetComponent<Actor>();

            _copyActorScript.KakashiCopied(MyTeam);

            _copyActor.transform.position = this.transform.position;
            _copyActor.transform.localScale = this.transform.localScale;

            photonView.RPC("ActorCopy", RpcTarget.All, _copyActor.GetPhotonView().ViewID, true);

            OnSkillCoroutine = DoCopyActor();
            StartCoroutine(OnSkillCoroutine);
        }

        private IEnumerator DoCopyActor()
        {
            float lifetime = 0f;

            while (lifetime < 10f && !_isAttackInput)
            {
                lifetime += Time.deltaTime;
                yield return null;
            }

            photonView.RPC("ActorCopy", RpcTarget.All, _copyActor.GetPhotonView().ViewID, false);

            ReturnKakashi();

            DestroyCopiedActor();

            OnSkillCoroutine = null;
        }

        private void ReturnKakashi()
        {
            base.Start();

            if (_copyActor != null)
            {
                this.transform.position = _copyActor.transform.position;
                this.transform.localScale = _copyActor.transform.localScale;
            }

            _smoothSync.teleport();
        }

        private void DestroyCopiedActor()
        {
            if (_copyActor != null)
            {
                if (_copyActorScript != null)
                {
                    _copyActorScript.DeadAction?.Invoke();
                }

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

            var newSmoke = Global.PoolingManager.LocalSpawn("Naruto_Smoke", _copyActor.transform.position,
                Quaternion.identity, true);
        }

        [PunRPC]
        public void ThrowShuriken(Vector3 shurikenPosition, Vector3 shurikenDir)
        {
            var newShuriken = Global.PoolingManager.LocalSpawn("Kakashi_Shuriken", this.transform.position,
                Quaternion.identity, true);
            var shurikenScript = newShuriken.GetComponent<Kakashi_Shuriken>();
            shurikenScript.SetInfo(this.photonView, this.gameObject, shurikenPosition, shurikenDir, MyTeam);

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
            var targetShuriken = _shurikenList[count];

            _shurikenList.Remove(targetShuriken);

            Global.PoolingManager.LocalDespawn(targetShuriken.gameObject);
        }

        [PunRPC]
        public void Sharingan()
        {
            var newEye = Global.PoolingManager.LocalSpawn("Kakashi_EyeCanvas", this.transform.position,
                Quaternion.identity, true);
        }

        protected override void Dead()
        {
            ReturnKakashi();

            base.Dead();

            DestroyCopiedActor();

            _copyActor = null;
            _copyActorScript = null;
        }
    }
}