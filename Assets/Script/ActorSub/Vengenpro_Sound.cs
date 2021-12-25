using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WakHead;

public class Vengenpro_Sound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource.outputAudioMixerGroup = Global.SoundManager.MainAudioMixer.FindMatchingGroups("Master/Effect")[0];
    }
}
