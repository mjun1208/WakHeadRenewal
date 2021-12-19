using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class UILife : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Image _lifeImage;
        
        private Actor _targetActor;
        private Tower _targetTower;
        
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
            
            switch (_team)
            {
                case Team.BLUE:
                {
                    _targetTower = Global.instance.BlueTower;
                    break;
                }
                case Team.RED:
                {
                    _targetTower = Global.instance.RedTower;
                    break;
                }
            }
        }
        
        
        private void Update()
        {
            if (_targetActor != null && _targetActor.IsLifeOn)
            {
                LifeUpdate(_targetActor.Life);
            }
            else if (_targetTower != null)
            {
                LifeUpdate(_targetTower.HP);
            }
        }

        public void LifeUpdate(int life)
        {
            if (life != 0)
            {
                _lifeImage.fillAmount = (float)life / 5f;
            }
            else
            {
                _lifeImage.fillAmount = 0f;
            }
        }
    }
}
