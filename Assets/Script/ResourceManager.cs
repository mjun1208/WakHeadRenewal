using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public bool IsLoaded { get; private set; } = false;

    private string _assetBundlePath = "Assets/AssetBundles/asset";
    private Object[] _loadedPrefabs;

    public void Load()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/asset";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/asset";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedPrefabs = bundle.LoadAllAssets();

        PoolingPrefabs();
    }

    public void PoolingPrefabs()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && _loadedPrefabs != null)
        {
            foreach (GameObject prefab in _loadedPrefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }

        IsLoaded = true;
    }

    public GameObject FindPrefab(string name)
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;

        if (!pool.ResourceCache.ContainsKey(name))
        {
            return null;
        }

        return pool.ResourceCache[name];
    }
}
