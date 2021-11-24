using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _skillIconImage;
        [SerializeField] private Image _skillCoolTimeIconImage;
        [SerializeField] private Image _skillOnIconImage;
        [SerializeField] private Image _skillIconEdgeImage;
        [SerializeField] private int _skillNum;

        private Actor _targetActor;
        private string _actorName;
        private bool _isOn = true;
        
        private void Start()
        {
            switch (_team)
            {
                case Team.BLUE:
                {
                    Global.instance.BlueActorSetAction += SetActor;
                    break;
                }
                case Team.RED:
                {
                    Global.instance.RedActorSetAction += SetActor;
                    break;
                }
            }
        }

        private void SetActor(Actor actor)
        {
            _targetActor = actor;

            if (Global.instance.MyTeam == _team)
            {
                _actorName = Global.instance.MyActorName;
            }
            else
            {
                _actorName = Global.instance.EnemyActorName;
            }

            var skillIconName = $"{_actorName}_Skill_{_skillNum}_Icon";

            _skillIconImage.sprite = Global.GameDataManager.FindSkillIcon(skillIconName);
            _skillCoolTimeIconImage.sprite = _skillIconImage.sprite;
        }

        private void Update()
        {
            if (_targetActor != null)
            {
                if (_skillNum == 1)
                {
                    SkillIconUpdate(_targetActor.Skill_1_Delay, _targetActor.Skill_1_CoolTime);
                }
                else
                {
                    SkillIconUpdate(_targetActor.Skill_2_Delay, _targetActor.Skill_2_CoolTime);
                }
            }
        }

        public void SkillIconUpdate(float delay, float coolTime)
        {
            if (delay > 0)
            {
                _skillIconEdgeImage.gameObject.SetActive(false);
                _skillCoolTimeIconImage.fillAmount = delay / coolTime;

                _isOn = false;
            }
            else
            {
                if (!_isOn)
                {
                    _skillOnIconImage.DOColor(new Color(1, 1, 1, 0.5f), 1f)
                        .From(new Color(1, 1, 1, 0f))
                        .OnComplete(()=>
                        {
                            _skillOnIconImage.DOColor(new Color(1, 1, 1, 0f), 1f);
                        });
                }
                        
                _skillIconEdgeImage.gameObject.SetActive(true);
                _skillCoolTimeIconImage.fillAmount = 0f;
            }
        }
    }
}