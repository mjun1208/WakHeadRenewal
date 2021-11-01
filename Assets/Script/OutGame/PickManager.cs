using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

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

    private const string ARTIST = "Artist. ";

    public Team MyTeam { get; private set; } = PhotonNetwork.IsMasterClient ? Team.BLUE : Team.RED;

    public int CurrentMyActorID { get; set; } = -1;
    public int CurrentEnemyActorID { get; set; } = -1;

    public bool IsMyReady { get; set; } = false;
    public bool IsEnemyReady { get; set; } = false;

    private PickSync _myPickSync;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Global.instance.FadeOut();

        var pickSync = PhotonNetwork.Instantiate("PickSync", Vector3.zero, Quaternion.identity);
        _myPickSync = pickSync.GetComponent<PickSync>();
    }

    public void ActorSelect(int index)
    {
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
        _redActor.Select(index);

        _redActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[index].KorName;
        _redActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[index].Artist;
    }

    public void Ready()
    {
        switch (MyTeam)
        {
            case Team.BLUE:
                {
                    _blueActor.Confirmed();
                    break;
                }
            case Team.RED:
                {
                    _redActor.Confirmed();
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
                    _redActor.Confirmed();
                    break;
                }
            case Team.RED:
                {
                    _blueActor.Confirmed();
                    break;
                }
        }
    }

    public void StartGame()
    {
        if (!IsMyReady || !IsEnemyReady)
        {
            return;
        }

        _myPickSync.StartGame();

        _pickUI.transform.DOLocalMoveY(-750, 1f).SetEase(Ease.InOutBack).OnComplete(()=>
        {
            Global.instance.FadeIn(() => PhotonNetwork.LoadLevel("Ingame"));
        });
    }

    public void StartGameClient()
    {
        _pickUI.transform.DOLocalMoveY(-750, 1f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            Global.instance.FadeIn();
        });
    }
}
