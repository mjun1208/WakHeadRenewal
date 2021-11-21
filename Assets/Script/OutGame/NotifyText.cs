using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NotifyText : MonoBehaviour
{
    [SerializeField] private Text _msgText;

    public void SetMsg(string text, float startY = 60f)
    {
        SetMsg(text, Color.red, startY);
    }

    public void SetMsg(string text, Color color, float startY = 60f)
    {
        _msgText.text = text;
        _msgText.color = color;
        this.transform.localPosition = new Vector3(0, startY);

        var targetColor = new Color(color.r, color.g, color.b, 0f);

        _msgText.DOColor(targetColor, 3f).SetEase(Ease.InCirc);
        this.transform.DOMoveY(20, 3f).SetEase(Ease.InCirc).SetRelative().
            OnComplete(() => Global.PoolingManager.LocalDespawn(this.gameObject));
    }
}
