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
                if (Skill_1_Delay > 0)
                {
                    Skill_1_Delay -= Time.deltaTime;
                }
                else
                {
                    Skill_1_Delay = 0;
                }
            
                if (Skill_2_Delay > 0)
                {
                    Skill_2_Delay -= Time.deltaTime;
                }
                else
                {
                    Skill_2_Delay = 0;
                }

                if (Flash_Delay > 0)
                {
                    Flash_Delay -= Time.deltaTime;
                }
                else
                {
                    Flash_Delay = 0;
                }

                KeyInput();
                
                this.transform.position = _copyActor.transform.position;
                
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

            Entity vowEntitiy = null;
            
            _skill_1Range.Attack(targetEntity => 
            {
                if (vowEntitiy == null)
                {
                    vowEntitiy = targetEntity;
                }
                
                targetEntity.KnockBack(10, GetAttackDir(), 1f, 0, MyTeam, "KakashiSkill_1Effect",0, Random.Range(0, 2) == 0);
            }, MyTeam);

            if (vowEntitiy != null)
            {
                StartCoroutine(ChainLightning(vowEntitiy.transform.position));   
            }
        }

        private IEnumerator ChainLightning(Vector3 targetPos)
        {
            List<Entity> chainedTargetEntityList = new List<Entity>();

            var carrierEntityPos = targetPos;
            var chainTargetEntity = GetChainTarget(carrierEntityPos, chainedTargetEntityList);

            while (chainTargetEntity != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    chainedTargetEntityList.Add(chainTargetEntity);

                    if (chainTargetEntity != null)
                    {
                        photonView.RPC("SpawnChainLightningEffect", RpcTarget.All,
                            carrierEntityPos.x, carrierEntityPos.y,
                            chainTargetEntity.transform.position.x, chainTargetEntity.transform.position.y);

                        carrierEntityPos = chainTargetEntity.transform.position;

                        chainTargetEntity.KnockBack(5, GetAttackDir(), 0.5f, 0, MyTeam, "KakashiSkill_1Effect", 0,
                            Random.Range(0, 2) == 0);

                        chainTargetEntity = GetChainTarget(carrierEntityPos, chainedTargetEntityList);
                    }
                    else
                    {
                        chainTargetEntity = null;
                    }
                }

                yield return new WaitForSeconds(0.3f);
            }

            yield return null;
        }

        [PunRPC]
        public void SpawnChainLightningEffect(float carrier_x, float carrier_y, float target_x, float target_y)
        {
            var newChainLightning = Global.PoolingManager.LocalSpawn("Kakashi_ChainLightning", new Vector3(carrier_x, carrier_y), Quaternion.identity, true);
            newChainLightning.GetComponent<Kakashi_ChainLightning>().SetInfo(new Vector3(carrier_x, carrier_y), new Vector3(target_x, target_y)); 
        }

        public Entity GetChainTarget(Vector3 carrierEntityPos, List<Entity> chainedTargetEntityList)
        {
            int layerMask = (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.CircleCastAll(carrierEntityPos, 2f, Vector2.zero, 0f, layerMask);

            foreach (var hit in hits)
            {
                var hitEntity = hit.transform.GetComponent<Entity>();
                if (chainedTargetEntityList.Contains(hitEntity) || hitEntity.MyTeam == MyTeam)
                {
                    continue;
                }

                return hitEntity;
            }

            return null;
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

            DestroyCopiedActor();
            
            ReturnKakashi();

            OnSkillCoroutine = null;
        }

        private void ReturnKakashi()
        {
            base.Start();

            if (photonView.IsMine)
            {
                CameraManager.instance.SetTarget(this.transform);
            }

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