using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using WakHead;

public class SoundVolumeSlider : MonoBehaviour
{
    private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _effectSlider;

    public void Awake()
    {
        _masterSlider.value = 0;
        _bgmSlider.value = 0;
        _effectSlider.value = 0;
        
        _audioMixer = Global.SoundManager.MainAudioMixer;
        
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _effectSlider.onValueChanged.AddListener(SetEffectVolume);
    }
    
    public void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat("Master", value);
    }

    public void SetBGMVolume(float value)
    {
        _audioMixer.SetFloat("BGM", value);
    }
    
    public void SetEffectVolume(float value)
    {
        _audioMixer.SetFloat("Effect", value);
    }
}
