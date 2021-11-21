using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIFindPlayer : MonoBehaviour
{
    [SerializeField] private Text _findPlayerText;
    
    private void Start()
    {
        TextAnimation();
    }

    public void TextAnimation()
    {
        _findPlayerText.DOText("플레이어 찾는 중.....", 1f).From("플레이어 찾는 중", true, false)
            .OnComplete(TextAnimation);
    }
}
