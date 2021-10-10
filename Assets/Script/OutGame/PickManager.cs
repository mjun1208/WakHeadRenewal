using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickManager : MonoBehaviour
{
    [SerializeField] private GameObject _pickUI;

    [SerializeField] private SimpleActor _blueActor;
    [SerializeField] private SimpleActor _redActor;

    [SerializeField] private Text _blueActorName;
    [SerializeField] private Text _redActorName;

    [SerializeField] private Text _blueActorArtist;
    [SerializeField] private Text _redActorArtist;

    private const string ARTIST = "Artist. ";

    public void Blue_Select(int index)
    {
        _blueActor.Select(index);

        _blueActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[index].KorName;
        _blueActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[index].Artist;
    }

    public void Red_Select(int index)
    {
        _redActor.Select(index);

        _redActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[0].KorName;
        _redActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[0].Artist;
    }

    public void Confirmed()
    {
        _blueActor.Confirmed();
        _pickUI.transform.DOLocalMoveY(-750, 1f).SetEase(Ease.InOutBack);
    }
}
