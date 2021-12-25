using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.UI;
using WakHead;

public class SoundManager : MonoBehaviour
{
    public bool IsLoaded { get; private set; } = false;
    public AudioMixer MainAudioMixer;
    
    private string _assetBundlePath = "Assets/AssetBundles/sound";
    private Object[] _loadedSounds;
    private Object[] _loadedIngameSounds;
    private Object[] _loadedDeathMatchSounds;
    
    private AudioMixerGroup _effectGroup;

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
        
        IngameLoad();
        DeathMatchLoad();
        AudioMixerLoad();
    }
    
    public void IngameLoad()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/ingamesound";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/ingamesound";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedIngameSounds = bundle.LoadAllAssets();
    }

    public void DeathMatchLoad()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/deathmatchsound";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/deathmatchsound";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        _loadedDeathMatchSounds = bundle.LoadAllAssets();
    }

    
    public void AudioMixerLoad()
    {
#if UNITY_EDITOR
        _assetBundlePath = "Assets/StreamingAssets/audiomixer";
#else
        _assetBundlePath = Application.dataPath + "/StreamingAssets/audiomixer";
#endif
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(_assetBundlePath));
        AssetBundle bundle = request.assetBundle;

        MainAudioMixer = bundle.LoadAsset("audiomixer") as AudioMixer;
        
        _effectGroup = MainAudioMixer.FindMatchingGroups("Master/Effect")[0];
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
    
    public AudioClip GetRandomIngameSound()
    {
        return _loadedIngameSounds[Random.Range(0, _loadedIngameSounds.Length)] as AudioClip;
    }
    
    public AudioClip GetRandomDeathMatchSound()
    {
        return _loadedDeathMatchSounds[Random.Range(0, _loadedDeathMatchSounds.Length)] as AudioClip;
    }
    
    public void Play(string name, Vector3 pos)
    {
        var audioSound = Global.PoolingManager.LocalSpawn("AudioSound", pos, Quaternion.identity);
        audioSound.GetComponent<AudioSound>().SetSource(FindSound(name), _effectGroup);
    }
}
