using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace WakHead
{
    public class Global : MonoBehaviourPunCallbacks
    {
        public static Global instance
        {
            get
            {
                if (_instance == null)
                {
                    return null;
                }

                return _instance;
            }
        }

        public static GameDataManager GameDataManager => _gameDataManager;
        public static ResourceManager ResourceManager => _resourceManager;
        public static PoolingManager PoolingManager => _poolingManager;

        private static Global _instance;
        private static GameDataManager _gameDataManager;
        private static ResourceManager _resourceManager;
        private static PoolingManager _poolingManager;

        public ObscuredString PlayerName { get; private set; }
        public ObscuredString EnemyName { get; private set; }

        public ObscuredString MyActorName { get; private set; }
        public ObscuredString EnemyActorName { get; private set; }

        public ObscuredInt MyActorID { get; private set; }
        public ObscuredInt EnemyActorID { get; private set; }

        public Tower RedTower { get; private set; }
        public Tower BlueTower { get; private set; }

        public Actor MyActor { get; private set; }
        public Actor EnemyActor { get; private set; }

        public Team MyTeam { get; private set; }
        public Team EnemyTeam { get; private set; }

        public Action<Actor> BlueActorSetAction;
        public Action<Actor> RedActorSetAction;

        public bool IsLoaded { get; private set; } = false;

        public Transform GlobalCanvas;
        [SerializeField] private Image _fadeUI;

        private bool _isLeaving = false;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);

                _gameDataManager = new GameDataManager();
                _resourceManager = new ResourceManager();
                _poolingManager = new PoolingManager();

                _gameDataManager.Load();
                _resourceManager.Load();

                IsLoaded = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("RealTitle");
            SceneManager.sceneLoaded += Leaving;
        }
        
        public void LeaveRoom()
        {
            ResetInfo();

            if (!_isLeaving)
            {
                _isLeaving = true;
                PhotonNetwork.LeaveRoom();
            }
        }

        public void Leaving(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= Leaving;
            
            _isLeaving = false;
        }

        public void ResetInfo()
        {
            EnemyName = null;

            MyActorName = null;
            EnemyActorName = null;

            MyActorID = 0;
            EnemyActorID = 0;

            RedTower = null;
            BlueTower = null;

            MyActor = null;
            EnemyActor = null;
            
            MyTeam = Team.None;
            EnemyTeam = Team.None;

            BlueActorSetAction = null;
            RedActorSetAction = null;
        }

        public void FadeIn(TweenCallback callback = null)
        {
            _fadeUI.DOKill();
            _fadeUI.enabled = true;
            _fadeUI.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(callback);
        }

        public void FadeOut(TweenCallback callback = null)
        {
            callback += () => { _fadeUI.enabled = false; };
            _fadeUI.DOKill();
            _fadeUI.DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(callback);
        }

        public void SetPlayerName(string name)
        {
            PlayerName = name;
        }

        public void SetEnemyName(string name)
        {
            EnemyName = name;
        }


        public void SetMyActorName(string name)
        {
            MyActorName = name;
        }

        public void SetEnemyActorName(string name)
        {
            EnemyActorName = name;
        }

        public void SetMyActorID(int id)
        {
            MyActorID = id;
        }

        public void SetEnemyActorID(int id)
        {
            EnemyActorID = id;
        }

        public void SetRedTower(Tower tower)
        {
            RedTower = tower;
        }

        public void SetBlueTower(Tower tower)
        {
            BlueTower = tower;
        }

        public void SetMyActor(Actor actor)
        {
            if (MyActor != null)
            {
                return;
            }

            MyActor = actor;
            MyTeam = PhotonNetwork.IsMasterClient ? Team.BLUE : Team.RED;
            MyActor.SetTeam(MyTeam);

            Action<Actor> actorSetAction = PhotonNetwork.IsMasterClient ? BlueActorSetAction : RedActorSetAction;
            actorSetAction?.Invoke(MyActor);
        }

        public void SetEnemyActor(Actor actor)
        {
            if (EnemyActor != null)
            {
                return;
            }

            EnemyActor = actor;
            EnemyTeam = PhotonNetwork.IsMasterClient ? Team.RED : Team.BLUE;
            EnemyActor.SetTeam(EnemyTeam);

            Action<Actor> actorSetAction = PhotonNetwork.IsMasterClient ? RedActorSetAction : BlueActorSetAction;
            actorSetAction?.Invoke(EnemyActor);
        }
    }
}