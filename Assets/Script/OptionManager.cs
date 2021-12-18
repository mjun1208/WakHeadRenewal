using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject _optionWindow;
    
    private FullScreenMode _screenMode;

    private void Awake()
    {
        _screenMode = Screen.fullScreenMode;
    }

    public void ShowOptionWindow()
    {
        _optionWindow.SetActive(!_optionWindow.activeSelf);
    }
    
    public void SetScreenResolution(int screen_x, int screen_y)
    {
        Screen.SetResolution(screen_x, screen_y, _screenMode);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _screenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreenMode = _screenMode;
    }
}
