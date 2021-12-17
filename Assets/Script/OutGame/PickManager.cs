using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace WakHead
{
    public class PickManager : MonoBehaviour
    {
        public static PickManager Instance;

        [SerializeField] private GameObject _pickUI;

        [SerializeField] private SimpleActor _blueActor;
        [SerializeField] private SimpleActor _redActor;

        [SerializeField] private Text _blueActorName;
        [SerializeField] private Text _redActorName;

        [SerializeField] private Text _blueActorArtist;
        [SerializeField] private Text _redActorArtist;
        
        [SerializeField] private GameObject _blueReady;
        [SerializeField] private GameObject _redReady;
        
        [SerializeField] private GameObject _readyButton;

        private const string ARTIST = "Artist. ";

        public Team MyTeam { get; private set; } = PhotonNetwork.IsMasterClient ? Team.BLUE : Team.RED;

        public int CurrentMyActorID { get; set; } = -1;
        public int CurrentEnemyActorID { get; set; } = -1;

        public bool IsMyReady
        {
            get
            {
                return _isMyReady;
            }
            set 
            {
                if (_isMyReady != value)
                {
                    _isMyReady = value;
                    
                    if (value && IsEnemyReady && PhotonNetwork.IsMasterClient)
                    {
                        StartGame();
                    }
                }
            }
        }

        private bool _isMyReady = false;

        public bool IsEnemyReady
        {
            get
            {
                return _isEnemyReady;
            }
            set
            {
                if (_isEnemyReady != value)
                {
                    _isEnemyReady = value;
                    
                    if (value && IsMyReady && PhotonNetwork.IsMasterClient)
                    {
                        StartGame();
                    }
                }
            }
        }
        
        private bool _isEnemyReady = false;

        public PickSync MyPickSync { get; set; }

        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Global.instance.FadeOut();
            
            Global.instance.SetMyTeam(PhotonNetwork.IsMasterClient ? Team.BLUE : Team.RED);
            Global.instance.SetEnemyTeam(PhotonNetwork.IsMasterClient ? Team.RED : Team.BLUE);

            var pickSync = PhotonNetwork.Instantiate("PickSync", Vector3.zero, Quaternion.identity);
            MyPickSync = pickSync.GetComponent<PickSync>();
        }

        public void ActorSelect(int index)
        {
            if (IsMyReady)
            {
                return;
            }

            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    Blue_Select(index);
                    break;
                }
                case Team.RED:
                {
                    Red_Select(index);
                    break;
                }
            }

            CurrentMyActorID = index;
        }

        public void EnemyActorSelect(int index)
        {
            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    Red_Select(index);
                    break;
                }
                case Team.RED:
                {
                    Blue_Select(index);
                    break;
                }
            }

            CurrentEnemyActorID = index;
        }

        public void Blue_Select(int index)
        {
            _blueActor.Select(index);

            _blueActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[index].KorName;
            _blueActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[index].Artist;
        }

        public void Red_Select(int index)
        {
            switch (MyTeam)
            {
                case Team.RED:
                {
                    if (IsMyReady)
                    {
                        return;
                    }
                    break;
                }
            }
            
            _redActor.Select(index);

            _redActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[index].KorName;
            _redActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[index].Artist;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IsMyReady = true;
                IsEnemyReady = true;
                StartGame();
            }
        }

        public void Ready()
        {
            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    _blueActor.Confirmed();
                    _blueReady.SetActive(true);
                    break;
                }
                case Team.RED:
                {
                    _redActor.Confirmed();
                    _redReady.SetActive(true);
                    break;
                }
            }

            IsMyReady = true;

            Global.instance.SetMyActorID(CurrentMyActorID);
        }

        public void EnemyReady()
        {
            switch (MyTeam)
            {
                case Team.BLUE:
                {
                    _redReady.SetActive(true);
                    // _redActor.Confirmed();
                    break;
                }
                case Team.RED:
                {
                    _blueReady.SetActive(true);
                    // _blueActor.Confirmed();
                    break;
                }
            }
        }

        public void StartGame()
        {
            if (!IsMyReady || !IsEnemyReady || !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            MyPickSync.StartGame();

            _pickUI.transform.DOLocalMoveY(-750, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Global.instance.GameReady();
                
                Global.instance.FadeIn(() =>
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel("Ingame");
                    }
                });
            });
        }

        public void StartGameClient()
        {
            _pickUI.transform.DOLocalMoveY(-750, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                Global.instance.GameReady();
                
                Global.instance.FadeIn();
            });
        }
    }
}