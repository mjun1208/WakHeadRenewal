using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace WakHead
{
    public class IngameTimer : MonoBehaviour
    {
        [SerializeField] private GameObject _blueTower;
        [SerializeField] private GameObject _redTower;
        [SerializeField] private GameObject _occupiedAreaManager;
        
        [SerializeField] private SpriteRenderer _sea;
        [SerializeField] private SpriteRenderer _cloud;
        [SerializeField] private SpriteRenderer _filter;

        [SerializeField] private GameObject _snow;

        [SerializeField] private Text _timerText;
        [SerializeField] private Image _timerEdge;
        
        [SerializeField] private GameObject _blueLife;
        [SerializeField] private GameObject _redLife;

        private bool isTimerColorChanged = false;

        private void Awake()
        {
            Global.instance.GameStartAction += SetInfo;
        }

        public void SetInfo()
        {
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            int timer = 300;
            
            while (timer > 0)
            {
                yield return new WaitForSeconds(1f);
                timer--;

                int minute = timer / 60;
                int second = timer % 60;

                _timerText.text = string.Format("{0:00}:{1:00}", minute, second);

                if (timer <= 30 && !isTimerColorChanged)
                {
                    isTimerColorChanged = true;
                    
                    _timerEdge.DOColor(Color.red, 1f);
                }
            }

            _sea.DOColor(new Color(82f / 255f,  82f / 255f, 82f / 255f), 1f);
            _cloud.DOColor(new Color(128f / 255f,  128f / 255f, 128f / 255f, 91f / 255f), 1f);
            _filter.DOColor(new Color(6f / 255f,  0f, 38f / 255f, 91f / 255f), 1f);

            _blueTower.GetComponent<ChimpanzeeSpawner>().DespawnChimpanzeeAll();
            _redTower.GetComponent<ChimpanzeeSpawner>().DespawnChimpanzeeAll();
            _occupiedAreaManager.GetComponent<OccupiedAreaManager>().DespawnOccupiedAreaAll();
            
            _blueTower.SetActive(false);
            _redTower.SetActive(false);
            _occupiedAreaManager.SetActive(false);
            
            _snow.SetActive(true);

            Global.instance.MyActor.Respawn();
            Global.instance.MyActor.ForceStun(1f);
            Global.instance.MyActor.LifeOn();
            
            _blueLife.SetActive(true);
            _redLife.SetActive(true);
        }
    }
}