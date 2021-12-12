using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WakHead;

public class AudioSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void SetSource(AudioClip clip)
    {
        _audioSource.clip = clip;
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        _audioSource.Play();

        yield return new WaitUntil(() => !_audioSource.isPlaying);

        Global.PoolingManager.LocalDespawn(this.gameObject);
    }
}
