using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WakHead;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown _dropdown;
    // [SerializeField] private Transform _contents;
    [SerializeField] private GameObject _item;
    [SerializeField] private List<Vector2> _resolutionlist = new List<Vector2>();

    private void Awake()
    {
        _dropdown.options.Clear();

        _dropdown.onValueChanged.AddListener(OnValueChanged);
        
        foreach (var resolution in _resolutionlist)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = $"{resolution.x} * {resolution.y}";
            _dropdown.options.Add(optionData);
        }
    }
    
    private void OnValueChanged(int index)
    {
        Global.OptionManager.SetScreenResolution((int) _resolutionlist[index].x, (int) _resolutionlist[index].y);
    }

}
