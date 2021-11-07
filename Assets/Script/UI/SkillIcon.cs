using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private Image _skillIconImage;
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
    }
}
