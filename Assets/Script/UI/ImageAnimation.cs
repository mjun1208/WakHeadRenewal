using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour {

	public Sprite[] sprites; 
	public float SpritePerSec = 0.1f;
	public bool loop = true;
	public bool destroyOnEnd = false;

	private int index = 0;
	private Image image;
	private float time = 0f;

	void Awake() {
		image = GetComponent<Image> ();
	}

	void Update () {
		if (!loop && index == sprites.Length) return;
		time += Time.deltaTime;
		if (time < SpritePerSec) return;
		image.sprite = sprites [index];
		time = 0;
		index ++;
		if (index >= sprites.Length) {
			if (loop) index = 0;
			if (destroyOnEnd) Destroy (gameObject);
		}
	}
}