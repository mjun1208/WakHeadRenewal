using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorHpGauge : MonoBehaviour
{
    [SerializeField] private Image _hpGaugeImage;

    private void Update()
    {
        if (Global.instance.MyActor != null) {

            if (Global.instance.MyActor.HP > 0)
            {
                _hpGaugeImage.fillAmount = ((float)Global.instance.MyActor.HP / (float)Global.instance.MyActor.MaxHP);
            }
            else
            {
                _hpGaugeImage.fillAmount = 0f;
            }
        }
    }
}
