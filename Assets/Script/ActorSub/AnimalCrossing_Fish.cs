using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WakHead
{
    public class AnimalCrossing_Fish : ActorSub
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private List<Sprite> _fishSprites;

        private int _currentFishIndex = 0;

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);

            if (isActive)
            {
                _rigid.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
            }
        }

        public void SelectFish(int index)
        {
            _currentFishIndex = index;

            _renderer.sprite = _fishSprites[index];
        }

        public int GetMyFishIndex()
        {
            return _currentFishIndex;
        }

        public void SelectRandomFish()
        {
            var randomIndex = Random.Range(0, _fishSprites.Count);

            _currentFishIndex = randomIndex;

            _renderer.sprite = _fishSprites[randomIndex];
        }
    }
}