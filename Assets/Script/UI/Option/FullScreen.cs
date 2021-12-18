using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WakHead;

public class FullScreen : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    private void Awake()
    {
        _toggle.isOn = Screen.fullScreen;
        _toggle.onValueChanged.AddListener((bool isFullScreen) => Global.OptionManager.SetFullScreen(isFullScreen));
    }
}
