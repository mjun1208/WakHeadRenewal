using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lightList;
    [SerializeField] private AudioSource _bgm;

    public void PlayTitleAnime()
    {
        _bgm.Play();
    }

    public void OnJururuLight()
    {
        OffAllLight();
        _lightList[0].SetActive(true);
    }

    public void OnIneLight()
    {
        OffAllLight();
        _lightList[1].SetActive(true);
    }

    public void OnGoseguLight()
    {
        OffAllLight();
        _lightList[2].SetActive(true);
    }

    public void OnViichanLight()
    {
        OffAllLight();
        _lightList[3].SetActive(true);
    }

    public void OnJingburgerLight()
    {
        OffAllLight();
        _lightList[4].SetActive(true);
    }

    public void OnLilpaLight()
    {
        OffAllLight();
        _lightList[5].SetActive(true);
    }

    public void OffAllLight()
    {
        foreach(var light in _lightList)
        {
            light.SetActive(false);
        }
    }

    public void OnAllLight()
    {
        foreach (var light in _lightList)
        {
            light.SetActive(true);
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("Ingame");
    }
}
