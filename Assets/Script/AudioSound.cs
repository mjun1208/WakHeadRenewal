using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using WakHead;

public class AudioSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void SetSource(AudioClip clip, AudioMixerGroup mixer)
    {
        _audioSource.clip = clip;
        _audioSource.outputAudioMixerGroup = mixer;

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        _audioSource.PlayOneShot(_audioSource.clip);

        yield return new WaitUntil(() => !_audioSource.isPlaying);

        _audioSource.Stop();
        _audioSource.clip = null;
        
        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
