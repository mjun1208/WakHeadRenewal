using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickManager : MonoBehaviour
{
    [SerializeField] private SimpleActor _blueActor;
    [SerializeField] private SimpleActor _redActor;

    [SerializeField] private Text _blueActorName;
    [SerializeField] private Text _redActorName;

    [SerializeField] private Text _blueActorArtist;
    [SerializeField] private Text _redActorArtist;

    private const string ARTIST = "Artist. ";

    public void Blue_Select(int index)
    {
        _blueActor.gameObject.SetActive(true);
        _blueActor.Select(index);

        _blueActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[index].KorName;
        _blueActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[index].Artist;
    }

    public void Red_Select(int index)
    {
        _redActor.gameObject.SetActive(true);
        _redActor.Select(index);

        _redActorName.text = Global.GameDataManager.ActorGameData.ActorGameDataList[0].KorName;
        _redActorArtist.text = ARTIST + Global.GameDataManager.ActorGameData.ActorGameDataList[0].Artist;
    }
}
