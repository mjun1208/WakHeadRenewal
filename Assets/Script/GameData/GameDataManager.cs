using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager
{
    public ActorGameDataArray ActorGameData { get; set; }

    private string _assetBundlePath = "Assets/AssetBundles/gamedata";

    public void Load()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/gamedata";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/gamedata";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        TextAsset data = bundle.LoadAsset("ActorData") as TextAsset;

        ActorGameData = JsonUtility.FromJson<ActorGameDataArray>(data.ToString());
    }
}
