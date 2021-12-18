using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class FlashIcon : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _skillIconImage;
        [SerializeField] private Image _skillCoolTimeIconImage;
        [SerializeField] private Image _skillOnIconImage;
        [SerializeField] private Image _skillIconEdgeImage;

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

        private void OnDestroy()
        {
            switch (_team)
            {
                case Team.BLUE:
                {
                    Global.instance.BlueActorSetAction -= SetActor;
                    break;
                }
                case Team.RED:
                {
                    Global.instance.RedActorSetAction -= SetActor;
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
            if (_targetActor != null && _targetActor.photonView.IsMine)
            {
                FlashIconUpdate(_targetActor.Flash_Delay, _targetActor.Flash_CoolTime);
            }
        }

        public void FlashIconUpdate(float delay, float coolTime)
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
                    _skillOnIconImage.DOColor(new Color(1, 1, 1, 0.5f), 0.2f)
                        .From(new Color(1, 1, 1, 0f))
                        .OnComplete(()=>
                        {
                            _skillOnIconImage.DOColor(new Color(1, 1, 1, 0f), 0.2f);
                        });

                    _isOn = true;
                }
                        
                _skillIconEdgeImage.gameObject.SetActive(true);
                _skillCoolTimeIconImage.fillAmount = 0f;
            }
        }
    }
}