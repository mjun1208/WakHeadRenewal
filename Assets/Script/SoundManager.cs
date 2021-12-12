using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WakHead;

public class SoundManager : MonoBehaviour
{
    public bool IsLoaded { get; private set; } = false;

    private string _assetBundlePath = "Assets/AssetBundles/sound";
    private Object[] _loadedSounds;

    public void Load()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/sound";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/sound";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedSounds = bundle.LoadAllAssets();
    }
    
    public AudioClip FindSound(string name)
    {
        foreach (var sound in _loadedSounds)
        {
            if (sound.name == name)
            {
                return sound as AudioClip;
            }
        }

        return null;
    }
    
    public void Play(string name, Vector3 pos)
    {
        var audioSound = Global.PoolingManager.LocalSpawn("AudioSound", pos, Quaternion.identity);
        audioSound.GetComponent<AudioSound>().SetSource(FindSound(name));
    }
}
