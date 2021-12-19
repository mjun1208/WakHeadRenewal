using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class ProfileIcon : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _profileIconImage;

        private Actor _targetActor;
        private string _actorName;

        private void Awake()
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

            var profileIconName = $"{_actorName}_Profile_Icon";

            _profileIconImage.sprite = Global.GameDataManager.FindProfileIcon(profileIconName);
        }
    }
}