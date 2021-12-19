using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class RespawnTimer : MonoBehaviour
    {
        private Actor _targetActor;
        [SerializeField] private GameObject _respawnTimerObject;
        [SerializeField] private Text _respawnTimerText;

        private float timer = 0f;
        
        private void Awake()
        {
            Global.instance.MyActorSetAction += SetActor;
        }

        private void SetActor(Actor actor)
        {
            _targetActor = actor;
            _targetActor.DeadAction += Show;
        }

        private void Show()
        {
            _respawnTimerObject.SetActive(true);
            timer = 5f + _targetActor.DeadTime * 2f;
            _respawnTimerText.text = ((int)timer).ToString();

            StartCoroutine(StartRespawnTimer());
        }

        private IEnumerator StartRespawnTimer()
        {
            _respawnTimerText.text = ((int)timer).ToString();

            while (timer > 0 && _targetActor.IsDead)
            {
                _respawnTimerText.text = ((int)timer).ToString();
                yield return new WaitForSeconds(1f);
                timer--;
            }
            
            _respawnTimerObject.SetActive(false);
            timer = 0f;
        }
    }
}