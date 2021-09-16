using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCrossing_Fish : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private List<Sprite> _fishSprites;

    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);

        if (isActive)
        {
            _rigid.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
        }
    }

    public void SelectRandomFish()
    {
        var randomIndex = Random.Range(0, _fishSprites.Count - 1);

        _renderer.sprite = _fishSprites[randomIndex];
    }
}
