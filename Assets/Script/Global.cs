﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public static GameDataManager GameDataManager => _gameDataManager;
    public static ResourceManager ResourceManager => _resourceManager;
    public static PoolingManager PoolingManager => _poolingManager;

    private static Global _instance;
    private static GameDataManager _gameDataManager;
    private static ResourceManager _resourceManager;
    private static PoolingManager _poolingManager;

    public string MyActorName { get; private set; }
    public string EnemyActorName { get; private set; }

    public Tower RedTower { get; private set; }
    public Tower BlueTower { get; private set; }

    [SerializeField] private Image _fadeUI;

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

        _gameDataManager = new GameDataManager();
        _resourceManager = new ResourceManager();
        _poolingManager = new PoolingManager();

        _gameDataManager.Load();
        _resourceManager.Load();
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
