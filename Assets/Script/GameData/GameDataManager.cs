﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager
{
    public ActorGameDataArray ActorGameData { get; set; }

    private Object[] _loadedSkillIcon;
    private Object[] _loadedProfilelIcon;

    public void Load()
    {
        GameDataLoad();
        SkillIconLoad();
        ProfileIconLoad();
    }

    public void GameDataLoad()
    {
        string assetBundlePath;

#if UNITY_EDITOR
        assetBundlePath = "Assets/StreamingAssets/gamedata";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/gamedata";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        TextAsset data = bundle.LoadAsset("ActorData") as TextAsset;

        ActorGameData = JsonUtility.FromJson<ActorGameDataArray>(data.ToString());
    }

    public void SkillIconLoad()
    {
        string assetBundlePath;

#if UNITY_EDITOR
        assetBundlePath = "Assets/StreamingAssets/skillicon";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/skillicon";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedSkillIcon = bundle.LoadAllAssets();
    }

    public void ProfileIconLoad()
    {
        string assetBundlePath;

#if UNITY_EDITOR
        assetBundlePath = "Assets/StreamingAssets/profileicon";
#else
        assetBundlePath = Application.dataPath + "/StreamingAssets/profileicon";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedProfilelIcon = bundle.LoadAllAssets();
    }

    public Sprite FindSkillIcon(string name)
    {
        foreach (var skillIcon in _loadedSkillIcon)
        {
            if (skillIcon.name == name)
            {
                var skillIconTex = skillIcon as Texture2D;

                Sprite skillIconSprite = Sprite.Create(skillIconTex, new Rect(0.0f, 0.0f, skillIconTex.width, skillIconTex.height), new Vector2(0.5f, 0.5f), 100.0f);

                return skillIconSprite as Sprite;
            }
        }

        return null;
    }

    public Sprite FindProfileIcon(string name)
    {
        foreach (var profileIcon in _loadedProfilelIcon)
        {
            if (profileIcon.name == name)
            {
                var profileIconTex = profileIcon as Texture2D;

                Sprite profileIconSprite = Sprite.Create(profileIconTex, new Rect(0.0f, 0.0f, profileIconTex.width, profileIconTex.height), new Vector2(0.5f, 0.5f), 100.0f);

                return profileIconSprite as Sprite;
            }
        }

        return null;
    }
}
