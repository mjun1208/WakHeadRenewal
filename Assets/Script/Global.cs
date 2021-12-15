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
        public static SoundManager SoundManager => _soundManager;

        private static Global _instance;
        private static GameDataManager _gameDataManager;
        private static ResourceManager _resourceManager;
        private static PoolingManager _poolingManager;
        private static SoundManager _soundManager;

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

        public Action<Actor> MyActorSetAction;
        
        public Action<Actor> BlueActorSetAction;
        public Action<Actor> RedActorSetAction;

        public bool IsLoaded { get; private set; } = false;

        public Transform GlobalCanvas;
        [SerializeField] private Image _fadeUI;
        [SerializeField] private GameObject _blueWinImage;
        [SerializeField] private GameObject _redWinImage;

        [SerializeField] private GameObject _readyObject;
        
        [SerializeField] private GameObject _blueReadyObject;
        [SerializeField] private GameObject _redReadyObject;
        
        [SerializeField] private Image _blueReadyActorImage;
        [SerializeField] private Image _redReadyActorImage;
        
        [SerializeField] private Text _blueReadyNameText;
        [SerializeField] private Text _redReadyNameText;
        
        private bool _isGameStarted = false;
        private bool _isLeaving = false;
        private bool _isEndGame = false;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);

                _gameDataManager = new GameDataManager();
                _resourceManager = new ResourceManager();
                _poolingManager = new PoolingManager();
                _soundManager = new SoundManager();

                _gameDataManager.Load();
                _resourceManager.Load();
                _soundManager.Load();

                IsLoaded = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (!_isEndGame)
            {
                LeaveRoom();
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("RealTitle");
            SceneManager.sceneLoaded += Leaving;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !_isGameStarted)
            {
                GameStart();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LeaveRoom();
            }
        }

        public void LeaveRoom()
        {
            ResetInfo();

            if (!_isLeaving)
            {
                _isLeaving = true;
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
            }
        }

        public void Leaving(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= Leaving;
            
            _blueWinImage.SetActive(false);
            _redWinImage.SetActive(false);
            
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

            MyActorSetAction = null;
            BlueActorSetAction = null;
            RedActorSetAction = null;

            _isEndGame = false;
        }

        public void WinTeam(Team team)
        {
            if (_isEndGame)
            {
                return;
            }
            
            _isEndGame = true;
            Invoke("LeaveRoom", 2f);

            switch (team)
            {
                case Team.BLUE:
                {
                    _blueWinImage.SetActive(true);
                    break;
                }
                case Team.RED:
                {
                    _redWinImage.SetActive(true);
                    break;
                }
            }
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
        
        public void GameReady()
        {
            _readyObject.SetActive(true);

            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    _blueReadyActorImage.sprite = GameDataManager.FindBodyImage(GameDataManager.ActorGameData.ActorGameDataList[MyActorID].Name);
                    _redReadyActorImage.sprite = GameDataManager.FindBodyImage(GameDataManager.ActorGameData.ActorGameDataList[EnemyActorID].Name);

                    _blueReadyNameText.text = PlayerName;
                    _redReadyNameText.text = EnemyName;
                    
                    break;
                } 
                case Team.RED:
                {
                    _blueReadyActorImage.sprite = GameDataManager.FindBodyImage(GameDataManager.ActorGameData.ActorGameDataList[EnemyActorID].Name);
                    _redReadyActorImage.sprite = GameDataManager.FindBodyImage(GameDataManager.ActorGameData.ActorGameDataList[MyActorID].Name);

                    _blueReadyNameText.text = EnemyName;
                    _redReadyNameText.text = PlayerName;
                    break;
                } 
            }

            _blueReadyObject.GetComponent<RectTransform>().DOAnchorPosX(553, 0.5f).From(new Vector2(-800, 0));
            _redReadyObject.GetComponent<RectTransform>().DOAnchorPosX(-553, 0.5f).From(new Vector2(800, 0));

            _isGameStarted = false;
        }

        public void GameStart()
        {
            _isGameStarted = true;
            
            _blueReadyObject.GetComponent<RectTransform>().DOAnchorPosX(-800, 0.5f).From(new Vector2(553, 0));
            _redReadyObject.GetComponent<RectTransform>().DOAnchorPosX(800, 0.5f).From(new Vector2(-553, 0)).OnComplete(() => _readyObject.SetActive(false));
        }
        

        public void SetPlayerName(string name)
        {
            PlayerName = name;
        }

        public void SetEnemyName(string name)
        {
            EnemyName = name;
        }

        public void SetMyTeam(Team team)
        {
            MyTeam = team;
        }

        public void SetEnemyTeam(Team team)
        {
            EnemyTeam = team;
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
            MyActor.SetTeam(MyTeam);
            
            actor.Spawn();

            MyActorSetAction?.Invoke(MyActor);
            
            Action<Actor> actorSetAction = PhotonNetwork.IsMasterClient ? BlueActorSetAction : RedActorSetAction;
            actorSetAction?.Invoke(MyActor);

            if (MyActor && EnemyActor && !_isGameStarted)
            {
                Invoke("GameStart", 1.5f);
            }
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
            
            if (MyActor && EnemyActor && !_isGameStarted)
            {
                Invoke("GameStart", 1.5f);
            }
        }
    }
}