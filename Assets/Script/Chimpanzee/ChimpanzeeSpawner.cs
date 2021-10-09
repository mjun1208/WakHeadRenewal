using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimpanzeeSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private Team _team;

    private List<GameObject> _chimpanzeeList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(Vow());
    }

    private IEnumerator Vow()
    {
        while (true)
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient &&
                !string.IsNullOrWhiteSpace(Global.instance.MyActorName) &&
                !string.IsNullOrWhiteSpace(Global.instance.EnemyActorName))
            {
                Spawn();
            }

            yield return new WaitForSeconds(5f);
        }
    }

    public void Spawn()
    {
        string spawnName = "";

        switch (_team)
        {
            case Team.BLUE:
                {
                    spawnName = Constant.PANZEE_BLUE;
                    break;
                }
            case Team.RED:
                {
                    spawnName = Constant.PANZEE_RED;
                    break;
                }
        }

        var randomPos = (Vector3)Random.insideUnitCircle * 3f;

        var newPanzee = PhotonNetwork.Instantiate(spawnName, this.transform.position + randomPos, Quaternion.identity);
        newPanzee.GetComponent<Entity>().SetTeam(_team);

        _chimpanzeeList.Add(newPanzee);
    }
}
