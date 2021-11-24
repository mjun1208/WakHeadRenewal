using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _skillIconImage;
        [SerializeField] private Image _skillCoolTimeIconImage;
        [SerializeField] private int _skillNum;

        private Actor _targetActor;
        private string _actorName;

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
                    if (_targetActor.Skill_1_Delay > 0)
                    {
                        _skillCoolTimeIconImage.fillAmount = _targetActor.Skill_1_Delay / _targetActor.Skill_1_CoolTime;
                    }
                    else
                    {
                        _skillCoolTimeIconImage.fillAmount = 0f;
                    }
                }
                else
                {
                    if (_targetActor.Skill_2_Delay > 0)
                    {
                        _skillCoolTimeIconImage.fillAmount = _targetActor.Skill_2_Delay / _targetActor.Skill_2_CoolTime;
                    }
                    else
                    {
                        _skillCoolTimeIconImage.fillAmount = 0f;
                    }
                }
            }
        }
    }
}