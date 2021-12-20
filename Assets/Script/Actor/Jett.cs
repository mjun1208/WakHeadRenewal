using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace WakHead
{
    public class Jett : Actor
    {
        [SerializeField] private GameObject _ghostPivot;
        [SerializeField] private GameObject _operatorTrajectoryPivot;
        [SerializeField] private GameObject _skill2_0Pivot;
        [SerializeField] private GameObject _skill2_1Pivot;

        private ObscuredInt _shurikenCount = 0;

        protected override void Start()
        {
            base.Start();

            _attackMoveSpeed = 2f;
        }

        protected override void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                _isAttack = true;
            }
            else
            {
                _isAttack = false;
            }

            base.Update();
        }

        protected override void Active_Attack()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(_ghostPivot.transform.position, GetAttackDir(), 2f);
            Debug.DrawRay(_ghostPivot.transform.position, GetAttackDir() * 2f, Color.red, 3f);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    continue;
                }

                var entity = hit.transform.GetComponent<Entity>();
                if (entity != null)
                {
                    if (entity.MyTeam == MyTeam)
                    {
                        continue;
                    }
                    
                    entity.KnockBack(3, GetAttackDir(), 0.5f, 0, MyTeam, 
                        "JettAttackEffect", 0, GetAttackDir().x > 0);
                    break;
                }

                var summoned = hit.transform.GetComponent<Summoned>();
                if (summoned != null)
                {
                    if (summoned.MyTeam == MyTeam)
                    {
                        continue;
                    }
                    
                    summoned.Damaged(summoned.transform.position, MyTeam, 
                        "JettAttackEffect", 0, GetAttackDir().x > 0);
                    break;
                }
            }
        }

        protected override void Active_Skill_1()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            int layerMask = (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Summoned")) +
                            (1 << LayerMask.NameToLayer("Minion"));
            RaycastHit2D[] hits = Physics2D.RaycastAll(_operatorTrajectoryPivot.transform.position, GetAttackDir(), 10f,
                layerMask);
            Debug.DrawRay(_operatorTrajectoryPivot.transform.position, GetAttackDir() * 10f, Color.red, 3f);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    continue;
                }

                var entity = hit.transform.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.KnockBack(20, GetAttackDir(), 1f, 0, MyTeam, 
                        "JettSkill_1Effect",  GetAttackDir().x * 0.3f, GetAttackDir().x > 0);
                }

                var summoned = hit.transform.GetComponent<Summoned>();
                if (summoned != null)
                {
                    summoned.Damaged(summoned.transform.position, MyTeam,
                        "JettSkill_1Effect", GetAttackDir().x * 0.3f, GetAttackDir().x > 0);
                }
            }

            photonView.RPC("ShootOperator", RpcTarget.All);
        }

        protected override void Active_Skill_2()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            _shurikenCount += 10;
        }

        private void ThrowShuriken()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            photonView.RPC("ThrowShurikenRPC", RpcTarget.All, _shurikenCount % 2 == 0);
            _shurikenCount--;
        }

        protected override void Attack()
        {
            base.Attack();
            if (_shurikenCount == 0)
            {
                _animator.SetBool("IsAttack", _isAttackInput);

                _animator.SetBool("IsAttack_2_0", false);
                _animator.SetBool("IsAttack_2_1", false);
            }
            else
            {
                _animator.SetBool("IsAttack_2_0", _isAttackInput && _shurikenCount % 2 == 0);
                _animator.SetBool("IsAttack_2_1", _isAttackInput && _shurikenCount % 2 == 1);
            }
        }

        public override void PlayAttackSound()
        {
            base.PlayAttackSound();
            
            Global.SoundManager.Play("Jett_Attack_Sound" , this.transform.position);
        }

        public override void PlaySkill_1Sound()
        {
            base.PlaySkill_1Sound();
            
            Global.SoundManager.Play("Jett_Skill_1_Sound" , this.transform.position);
        }
        
        public override void PlaySkill_2Sound()
        {
            base.PlaySkill_2Sound();
            
            Global.SoundManager.Play("Jett_Skill_2_Sound" , this.transform.position);
        }
        
        public void PlaySkill_2_StartSound()
        {
            Global.SoundManager.Play("Jett_Skill_2_Start_Sound" , this.transform.position);
        }
        
        public void PlayShurikenSound()
        {
            Global.SoundManager.Play("Jett_Shuriken_Sound" , this.transform.position);
        }
        
        [PunRPC]
        public void ThrowShurikenRPC(bool isPivot0)
        {
            var throwPosition = isPivot0 ? _skill2_0Pivot : _skill2_1Pivot;

            var newShuriken = Global.PoolingManager.LocalSpawn("Jett_Shuriken", throwPosition.transform.position,
                Quaternion.identity, true);

            newShuriken.GetComponent<Jett_Shuriken>().SetInfo(this.photonView, this.gameObject,
                throwPosition.transform.position, GetAttackDir(), MyTeam);
        }

        [PunRPC]
        public void ShootOperator()
        {
            var newOperatorTrajectory = Global.PoolingManager.LocalSpawn("OperatorTrajectory",
                _operatorTrajectoryPivot.transform.position, Quaternion.identity, true);
            newOperatorTrajectory.GetComponent<SpriteRenderer>().flipX = GetAttackDir().x < 0;
        }
    }
}