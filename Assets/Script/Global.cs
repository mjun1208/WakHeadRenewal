using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
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

    public static ResourceManager ResourceManager => _resourceManager;
    public static PoolingManager PoolingManager => _poolingManager;

    private static Global _instance;
    private static ResourceManager _resourceManager;
    private static PoolingManager _poolingManager;

    public string MyActorName { get; private set; }
    public string EnemyActorName { get; private set; }

    public Tower RedTower { get; private set; }
    public Tower BlueTower { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        _resourceManager = new ResourceManager();
        _poolingManager = new PoolingManager();
        _resourceManager.Load();
    }

    public void SetMyActorName(string name)
    {
        MyActorName = name;
    }

    public void SetEnemyActorName(string name)
    {
        EnemyActorName = name;
    }

    public void SetRedTower(Tower tower)
    {
        RedTower = tower;
    }

    public void SetBlueTower(Tower tower)
    {
        BlueTower = tower;
    }
}
