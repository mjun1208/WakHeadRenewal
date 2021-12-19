using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class ActorHpGauge : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _hpGaugeImage;

        private Actor _targetActor;

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
        }

        private void Update()
        {
            if (_targetActor != null)
            {

                if (_targetActor.HP > 0)
                {
                    _hpGaugeImage.fillAmount = ((float) _targetActor.HP / (float) _targetActor.MaxHP);
                }
                else
                {
                    _hpGaugeImage.fillAmount = 0f;
                }
            }
        }
    }
}