using UnityEngine;
using WakHead;

public class IngameBGM : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    public void Awake()
    {
        Play();
    }

    private void Update()
    {
        if (!_source.isPlaying)
        {
            Play();
        }
    }

    public void Play()
    {
        _source.clip = Global.SoundManager.GetRandomIngameSound();
        _source.Play();
    }
}
