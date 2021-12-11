using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using WakHead;

public class ScreenHit : MonoBehaviour
{
    [SerializeField] private Image _screenHitImage;

    public void OnEnable()
    {
        _screenHitImage.DOColor(new Color(1, 1, 1, 0f), 0.5f).OnComplete(() =>
        {
            _screenHitImage.color = new Color(1, 1, 1, 1f);
            Global.PoolingManager.LocalDespawn(this.gameObject);
        });
    }
}
